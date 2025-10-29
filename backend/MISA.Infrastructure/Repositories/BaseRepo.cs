using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SalesManagement.BusinessLogic.Interfaces.Repository;
using SalesManagement.BusinessLogic.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.DataAccess.Repositories
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        protected readonly string connectionString;

        public BaseRepo(IConfiguration config)
        {
            connectionString = config.GetConnectionString("conn");
        }

        public List<T> GetAll()
        {
            var tableName = GetTableName();
            var sql = $"SELECT * FROM {tableName}";
            using var connection = new MySqlConnection(connectionString);
            return connection.Query<T>(sql).ToList();
        }

        public T Insert(T entity)
        {
            var tableName = GetTableName();

            var props = typeof(T).GetProperties()
                .Where(p => p.Name.ToLower() != "id" && p.GetValue(entity) != null)
                .ToList();

          
            var columns = string.Join(", ", props.Select(p => ToSnakeCase(p.Name)));
            var values = string.Join(", ", props.Select(p => $"@{p.Name}"));

            var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values});";

            using var connection = new MySqlConnection(connectionString);
            connection.Execute(sql, entity);

            return entity;
        }


        public T Update(string  id, T entity)
        {
            var tableName = GetTableName();

            var props = typeof(T).GetProperties()
                .Where(p => p.Name.ToLower() != "id" && p.GetValue(entity) != null)
                .ToList();

            var setClause = string.Join(", ", props.Select(p => $"{ToSnakeCase(p.Name)} = @{p.Name}"));
            var keyColumn = ToSnakeCase($"{typeof(T).Name}Id");

            var sql = $"UPDATE {tableName} SET {setClause} WHERE {keyColumn} = @Id;";

            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            connection.Execute(sql, new { Id = id, entity });

            // 2️⃣ Lấy lại entity vừa update
            var selectSql = $"SELECT * FROM {tableName} WHERE {keyColumn} = @Id;";
            var updatedEntity = connection.QuerySingleOrDefault<T>(selectSql, new { Id = id });

            return updatedEntity;
        }

        public int Delete(Guid id)
        {
            var tableName = GetTableName();
            var keyColumn = ToSnakeCase($"{typeof(T).Name}Id");

            var sql = $"DELETE FROM {tableName} WHERE {keyColumn} = @Id;";

            using var connection = new MySqlConnection(connectionString);
            return connection.Execute(sql, new { Id = id });
        }

        private string GetTableName()
        {
            var tableName = typeof(T).Name;
            var tableAttr = (MISATableNameAttribute?)Attribute.GetCustomAttribute(typeof(T), typeof(MISATableNameAttribute));
            if (tableAttr != null)
                tableName = tableAttr.TableName;

            // Chuyển sang snake_case
            return ToSnakeCase(tableName);
        }

        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) && i > 0)
                    sb.Append('_');
                sb.Append(char.ToLower(input[i]));
            }
            return sb.ToString();
        }
    }
}
