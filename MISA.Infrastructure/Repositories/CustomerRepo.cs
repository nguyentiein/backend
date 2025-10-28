using  SalesManagement.BusinessLogic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using SalesManagement.BusinessLogic.Interfaces.Repository;
using SalesManagement.BusinessLogic.Dtos;


namespace SalesManagement.DataAccess.Repositories
{
    public class CustomerRepo :BaseRepo<Customer>, ICustomerRepo
    {
        public CustomerRepo(IConfiguration config): base (config)
        {

        }


        public Customer RemoveCustomer(string customerCode)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

              
                var checkSql = "SELECT COUNT(*) FROM customer WHERE customer_code = @CustomerCode AND is_active = TRUE";
                var isExist = connection.ExecuteScalar<int>(checkSql, new { CustomerCode = customerCode }) > 0;

                if (!isExist)
                    throw new Exception($"Customer with code {customerCode} not found or already inactive");

      
                var updateSql = @"
            UPDATE customer
            SET is_active = FALSE,
                modified_date = @ModifiedDate
            WHERE customer_code = @CustomerCode;
        ";

                connection.Execute(updateSql, new { CustomerCode = customerCode, ModifiedDate = DateTime.Now });

                // 3️⃣ Trả về bản ghi sau khi "xóa mềm"
                var selectSql = "SELECT * FROM customer WHERE customer_code = @CustomerCode";
                var removedCustomer = connection.Query(selectSql, new { CustomerCode = customerCode })
                                                .Select(row => new Customer
                                                {
                                                    CustomerId = row.customer_id.ToString(),
                                                    CustomerCode = row.customer_code,
                                                    FullName = row.full_name,
                                                    CompanyName = row.company_name,
                                                    PhoneNumber = row.phone_number,
                                                    Address = row.address,
                                                    Email = row.email,
                                                    CustomerTypeId = row.customer_type_id,
                                                    CreatedDate = row.created_date,
                                                    ModifiedDate = row.modified_date,
                                                    DebtAmount = row.debt_amount,
                                                    LatestPurchaseDate = row.latest_purchase_date,
                                                 
                                                })
                                                .FirstOrDefault();

                return removedCustomer;
            }
        }






        public Customer UpdateCustomer(string customerCode, Customer customer)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var checkSql = "SELECT COUNT(*) FROM customer WHERE customer_code = @CustomerCode";
                var isExist = connection.ExecuteScalar<int>(checkSql, new { CustomerCode = customerCode }) > 0;

                if (!isExist)
                    throw new Exception($"Customer with code {customerCode} not found");

                var updates = new List<string>();
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(customer.FullName))
                {
                    updates.Add("full_name = @FullName");
                    parameters.Add("@FullName", customer.FullName);
                }

                if (!string.IsNullOrEmpty(customer.CompanyName))
                {
                    updates.Add("company_name = @CompanyName");
                    parameters.Add("@CompanyName", customer.CompanyName);
                }

                if (!string.IsNullOrEmpty(customer.PhoneNumber))
                {
                    updates.Add("phone_number = @PhoneNumber");
                    parameters.Add("@PhoneNumber", customer.PhoneNumber);
                }

                if (!string.IsNullOrEmpty(customer.Address))
                {
                    updates.Add("address = @Address");
                    parameters.Add("@Address", customer.Address);
                }

                if (!string.IsNullOrEmpty(customer.Email))
                {
                    updates.Add("email = @Email");
                    parameters.Add("@Email", customer.Email);
                }

                if (!string.IsNullOrEmpty(customer.CustomerTypeId))
                {
                    updates.Add("customer_type_id = @CustomerTypeId");
                    parameters.Add("@CustomerTypeId", customer.CustomerTypeId);
                }

                updates.Add("modified_date = @ModifiedDate");
                parameters.Add("@ModifiedDate", DateTime.Now);

                if (!updates.Any())
                    throw new Exception("No fields to update");

                var sql = $@"
            UPDATE customer
            SET {string.Join(", ", updates)}
            WHERE customer_code = @CustomerCode;
        ";

                parameters.Add("@CustomerCode", customerCode);
                connection.Execute(sql, parameters);
                var selectSql = "SELECT * FROM customer WHERE customer_code = @CustomerCode";
                var updatedCustomer = connection.Query(selectSql, new { CustomerCode = customerCode })
                                                .Select(row => new Customer
                                                {
                                                    CustomerId = row.customer_id.ToString(),
                                                    CustomerCode = row.customer_code,
                                                    FullName = row.full_name,
                                                    CompanyName = row.company_name,
                                                    PhoneNumber = row.phone_number,
                                                    Address = row.address,
                                                    Email = row.email,
                                                    CustomerTypeId = row.customer_type_id,
                                                    CreatedDate = row.created_date,
                                                    ModifiedDate = row.modified_date,
                                                    DebtAmount = row.debt_amount,
                                                    LatestPurchaseDate = row.latest_purchase_date
                                                })
                                                .FirstOrDefault();

                return updatedCustomer;
            }
        }


        public List<CustomerDto> GetCustomers()
        {
            string sqlCommand = @"
SELECT 
    ct.customer_type_name AS CustomerType,
    c.customer_code AS CustomerCode,
    c.full_name AS FullName,
    c.company_name AS CompanyName,
    c.phone_number AS PhoneNumber,
    MAX(cp.purchase_date) AS LatestPurchaseDate,
    GROUP_CONCAT(DISTINCT p.product_code ORDER BY p.product_code SEPARATOR ', ') AS PurchasedProductCodes,
    GROUP_CONCAT(DISTINCT p.product_name ORDER BY p.product_name SEPARATOR ', ') AS PurchasedProductNames,
    GROUP_CONCAT(DISTINCT sa.shipping_address ORDER BY sa.created_date SEPARATOR ' | ') AS ShippingAddresses
FROM customer c
LEFT JOIN customer_type ct ON c.customer_type_id = ct.customer_type_id
LEFT JOIN customer_purchase cp ON c.customer_id = cp.customer_id
LEFT JOIN purchase_item pi ON cp.purchase_id = pi.purchase_id
LEFT JOIN product p ON pi.product_id = p.product_id
LEFT JOIN shipping_address sa ON c.customer_id = sa.customer_id
GROUP BY 
    c.customer_id, 
    ct.customer_type_name, 
    c.customer_code, 
    c.full_name, 
    c.company_name, 
    c.phone_number
ORDER BY 
    c.created_date DESC;
";

            using (var connection = new MySqlConnection(connectionString))
            {
                return connection.Query<CustomerDto>(sqlCommand).ToList();
            }
        }


    }
}
