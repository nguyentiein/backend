using  SalesManagement.BusinessLogic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Interfaces.Repository
{
    public interface IBaseRepo<T>  where  T : class
    {
        /// <summary>
        /// Lấy  tất  cả
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetAll();

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm</param>
        /// <returns>Số bản ghi bị ảnh hưởng (thường là 1 nếu thêm thành công)</returns>
        T Insert(T entity);

        /// <summary>
        /// Cập nhật một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần cập nhật</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        T Update(string  id,T entity);

        /// <summary>
        /// Xóa một bản ghi theo ID
        /// </summary>
        /// <param name="id">ID của bản ghi cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        int Delete(Guid id);
    }
}
