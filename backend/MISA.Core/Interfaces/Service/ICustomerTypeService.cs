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
    public interface ICustomerTypeService
    {
        Task<PaginationResult<CustomerType>> GetCustomerType();
    }
}
