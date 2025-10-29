using AutoMapper;
using Microsoft.Extensions.Options;
using SalesManagement.BusinessLogic.Dtos;
using SalesManagement.BusinessLogic.Entities;
using SalesManagement.BusinessLogic.Interfaces.Repository;
using SalesManagement.BusinessLogic.Interfaces.Service;
using SalesManagement.BusinessLogic.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Services
{
    public  class CustomerTypeService : BaseService, ICustomerTypeService
    {

        ICustomerTypeRepo _customerRepo;
        public CustomerTypeService(ICustomerTypeRepo customerRepo, IMapper mapper,
        IOptionsMonitor<ResponseMessage> responseMessage) : base(mapper, responseMessage)
        {
            _customerRepo = customerRepo;
        }

        public async Task<PaginationResult<CustomerType>> GetCustomerType()
        {
            int page = 1;
            int pageSize = 2;
            var customers = _customerRepo.GetAll();
            var totalRecords = customers.Count;
            var pagedData = customers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<CustomerType>(pagedData, page, pageSize, totalRecords);
        }
    }
}
