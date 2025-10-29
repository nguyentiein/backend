using  SalesManagement.BusinessLogic.Core.Entities;
using SalesManagement.BusinessLogic.Dtos;
using SalesManagement.BusinessLogic.Entities;
using SalesManagement.BusinessLogic.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Interfaces.Service
{
    public interface ICustomerService
    {

        Task<PaginationResult<CustomerDto>> GetCustomers();
        Task<BaseResult<Customer>> InsertCustomer(Customer customer);
        Task<BaseResult<Customer>> UpdateCustomer(Customer customer,string id);
        Task<BaseResult<Customer>> DeleteCustomer(string  customerCode);
        Task<BaseResult<List<CustomerDto>>> GetCustomerByCustomerCode(string customerCode);
        Task<BaseResult<string>> GenerateCustomerCode();
        Task<PaginationResult<CustomerDto>> FilterCustomers(string? keyword);
        Task<BaseResult<List<Customer>>> ImportCustomers(Stream stream);
    }
}
