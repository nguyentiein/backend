using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SalesManagement.BusinessLogic.Core.Entities;
using SalesManagement.BusinessLogic.Dtos;
using SalesManagement.BusinessLogic.Dtos.Customer;
using SalesManagement.BusinessLogic.Entities;
using SalesManagement.BusinessLogic.Interfaces.Service;
using SalesManagement.BusinessLogic.Result;
using SalesManagement.BusinessLogic.Services;

namespace SalesManagement.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<BaseResult<List<CustomerDto>>> GetAllCustomer()
        {
            var result = await _customerService.GetCustomers();
            return result;
        }

        [HttpPost]
        public async Task<BaseResult<Customer>> InsertCustomer([FromBody] CreateCustomer dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            var result = await _customerService.InsertCustomer(customer);
            return result;
        }

        [HttpPatch("{id}")]
        public async Task<BaseResult<Customer>> UpdateCustomer(string  id,[FromBody] CreateCustomer updateRequest)
        {
            updateRequest.CustomerCode = id;
            var customer = _mapper.Map<Customer>(updateRequest);
            var result = await _customerService.UpdateCustomer(customer,id);
            return result;

        }

        [HttpDelete("{customerCode}")]
        public async Task<BaseResult<Customer>> DeleteCustomer(string  customerCode)
        {
            var result = await _customerService.DeleteCustomer(customerCode);
            return result;
        }

    }
}
