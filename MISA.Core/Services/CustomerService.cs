using AutoMapper;
using Microsoft.Extensions.Options;
using  SalesManagement.BusinessLogic.Core.Entities;
using SalesManagement.BusinessLogic.Dtos;
using SalesManagement.BusinessLogic.Entities;
using SalesManagement.BusinessLogic.Exceptions;
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
    public class CustomerService : BaseService, ICustomerService
    {
        ICustomerRepo _customerRepo;
        public CustomerService(ICustomerRepo customerRepo, IMapper mapper,
        IOptionsMonitor<ResponseMessage> responseMessage) : base(mapper, responseMessage) 
        {
            _customerRepo = customerRepo;
        }

        public async Task<PaginationResult<CustomerDto>> GetCustomers()
        {
            int page = 1;
            int pageSize = 2;
            var customers = _customerRepo.GetCustomers();
            var totalRecords = customers.Count;
            var pagedData = customers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<CustomerDto>(pagedData, page, pageSize, totalRecords);
        }

    
        public async Task<BaseResult<Customer>> UpdateCustomer(Customer customer, string id)
        {
            var updatedCustomer = _customerRepo.UpdateCustomer(id,customer);
            return GetBaseResult(CodeMessage._200, updatedCustomer);
        }

        public async Task<BaseResult<Customer>> InsertCustomer(Customer customer)
        {
            var insertedCustomer = _customerRepo.Insert(customer);
            return GetBaseResult(CodeMessage._200, insertedCustomer);
        }

        public async Task<BaseResult<Customer>> DeleteCustomer(string customerCode)
        {
            var deletedCustomer = _customerRepo.RemoveCustomer(customerCode);
            return GetBaseResult(CodeMessage._200, deletedCustomer);
        }

        public async Task<BaseResult<List<CustomerDto>>> GetCustomerByCustomerCode(string customerCode)
        {
            int page = 1;
            int pageSize = 2;
            var customers = _customerRepo.GetCustomersByCustomerCode(customerCode);
            var totalRecords = customers.Count;
            var pagedData = customers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<CustomerDto>(pagedData, page, pageSize, totalRecords);
        }

        public async Task<BaseResult<string>> GenerateCustomerCode()
        {
            string prefix = "KH" + DateTime.Now.ToString("yyyyMM");

            string latestCode = _customerRepo.GetLatestCustomerCode(prefix);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(latestCode) && latestCode.Length >= 14)
            {
                string lastNumberStr = latestCode.Substring(8); // lấy 6 số cuối
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            string newCode = prefix + nextNumber.ToString("D6");
            return GetBaseResult(CodeMessage._200, newCode);
        }


    }
}
