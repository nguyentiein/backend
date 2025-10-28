using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesManagement.BusinessLogic.Dtos;
using SalesManagement.BusinessLogic.Entities;
using SalesManagement.BusinessLogic.Interfaces.Service;
using SalesManagement.BusinessLogic.Result;

namespace SalesManagement.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerTypeController : ControllerBase
    {

        private readonly ICustomerTypeService _customerTypeService;
        private readonly IMapper _mapper;

        public CustomerTypeController(ICustomerTypeService customerTypeService, IMapper mapper)
        {
            _customerTypeService = customerTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<BaseResult<List<CustomerType>>> GetAllCustomer()
        {
            var result = await _customerTypeService.GetCustomerType();
            return result;
        }
    }
}
