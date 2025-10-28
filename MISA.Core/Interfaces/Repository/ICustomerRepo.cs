using  SalesManagement.BusinessLogic.Core.Entities;
using SalesManagement.BusinessLogic.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Interfaces.Repository
{
    public interface ICustomerRepo:IBaseRepo<Customer>
    {
        Customer UpdateCustomer(string custemerCode,Customer customer);
        Customer RemoveCustomer(string  customerCode);
        public List<CustomerDto> GetCustomers();
        public List<CustomerDto> GetCustomersByCustomerCode(string customerCode);
        string GetLatestCustomerCode(string prefix);
    }
}
