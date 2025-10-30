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
        /// <summary>
        /// Lấy danh sách khách hàng có phân trang.
        /// </summary>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="pageSize">Số bản ghi trên mỗi trang.</param>
        /// <returns>Danh sách khách hàng dạng DTO.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>

        [HttpGet]
        public async Task<BaseResult<List<CustomerDto>>> GetAllCustomer(int page, int pageSize)
        {
            var result = await _customerService.GetCustomers(page,pageSize);
            return result;
        }

        /// <summary>
        /// Lấy danh sách khách hàng có phân trang.
        /// </summary>
        /// <param name="customerCode">Mã KH.</param>
        /// <param name="pageSize">Số bản ghi trên mỗi trang.</param>
        /// <returns>Danh sách khách hàng dạng DTO.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>

        [HttpGet("{customerCode}")]
        public async Task<BaseResult<List<CustomerDto>>> GetCustomerByCustomerCode(string customerCode)
        {
            var result = await _customerService.GetCustomerByCustomerCode(customerCode);
            return result;
        }


        /// <summary>
        /// Thêm mới một khách hàng.
        /// </summary>
        /// <param name="dto">Đối tượng CreateCustomer chứa thông tin khách hàng cần thêm.</param>
        /// <returns>Đối tượng khách hàng sau khi được thêm vào hệ thống.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>

        [HttpPost]
        public async Task<BaseResult<Customer>> InsertCustomer([FromBody] CreateCustomer dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            var result = await _customerService.InsertCustomer(customer);
            return result;
        }
        /// <summary>
        /// Sinh mã khách hàng tự động theo quy tắc định sẵn.
        /// </summary>
        /// <returns>Chuỗi mã khách hàng mới (VD: CUS-00123).</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>

        [HttpGet("GenerateCustomerCode")]
        public async Task<BaseResult<string>> GenerateCustomerCode()
        {
            var result = await _customerService.GenerateCustomerCode();
            return result;
        }

        /// <summary>
        /// Cập nhật thông tin của một khách hàng dựa theo mã khách hàng.
        /// </summary>
        /// <param name="id">Mã khách hàng cần cập nhật.</param>
        /// <param name="updateRequest">Thông tin mới của khách hàng.</param>
        /// <returns>Đối tượng khách hàng sau khi cập nhật.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>

        [HttpPatch("{id}")]
        public async Task<BaseResult<Customer>> UpdateCustomer(string  id,[FromBody] CreateCustomer updateRequest)
        {
            updateRequest.CustomerCode = id;
            var customer = _mapper.Map<Customer>(updateRequest);
            var result = await _customerService.UpdateCustomer(customer,id);
            return result;

        }
        /// <summary>
        /// Xóa một khách hàng theo mã khách hàng.
        /// </summary>
        /// <param name="customerCode">Mã khách hàng cần xóa.</param>
        /// <returns>Kết quả sau khi xóa khách hàng.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>

        [HttpDelete("{customerCode}")]
        public async Task<BaseResult<Customer>> DeleteCustomer(string  customerCode)
        {
            var result = await _customerService.DeleteCustomer(customerCode);
            return result;
        }

        /// <summary>
        /// Lọc danh sách khách hàng theo từ khóa và phân trang.
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm (tên, mã, công ty,...).</param>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="pageSize">Số bản ghi mỗi trang.</param>
        /// <returns>Danh sách khách hàng thỏa mãn điều kiện lọc.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>
        [HttpGet("filter")]
        public async Task<BaseResult<List<CustomerDto>>> FilterCustomers([FromQuery] string? keyword, int page, int pageSize)
        {
            var result = await _customerService.FilterCustomers(keyword,  page,  pageSize);
            return result;
        }

        /// <summary>
        /// Import danh sách khách hàng từ file Excel.
        /// </summary>
        /// <param name="file">Tệp Excel chứa dữ liệu khách hàng.</param>
        /// <returns>Danh sách khách hàng được thêm từ file.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>
        [HttpPost("import")]
        public async Task<BaseResult<List<Customer>>> ImportCustomer(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var result = await _customerService.ImportCustomers(stream);
            return result;
        }

    }
}
