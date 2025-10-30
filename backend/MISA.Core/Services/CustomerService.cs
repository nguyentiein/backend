using AutoMapper;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using  SalesManagement.BusinessLogic.Core.Entities;
using SalesManagement.BusinessLogic.Dtos;
using SalesManagement.BusinessLogic.Dtos.Customer;
using SalesManagement.BusinessLogic.Entities;
using SalesManagement.BusinessLogic.Exceptions;
using SalesManagement.BusinessLogic.Interfaces.Repository;
using SalesManagement.BusinessLogic.Interfaces.Service;
using SalesManagement.BusinessLogic.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        /// <summary>
        /// Lấy danh sách khách hàng có hỗ trợ phân trang.
        /// </summary>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="pageSize">Số lượng bản ghi trên mỗi trang.</param>
        /// <returns>Danh sách khách hàng dạng DTO có thông tin phân trang.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>
        public async Task<PaginationResult<CustomerDto>> GetCustomers(int page ,int pageSize)
        {
         
            var customers = _customerRepo.GetCustomers();
            var totalRecords = customers.Count;
            var pagedData = customers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<CustomerDto>(pagedData, page, pageSize, totalRecords);
        }


        /// <summary>
        /// Cập nhật thông tin khách hàng theo mã định danh.
        /// </summary>
        /// <param name="customer">Đối tượng khách hàng chứa thông tin cập nhật.</param>
        /// <param name="id">Mã khách hàng cần cập nhật.</param>
        /// <returns>Kết quả chứa thông tin khách hàng sau khi cập nhật.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>
        public async Task<BaseResult<Customer>> UpdateCustomer(Customer customer, string id)
        {
            var updatedCustomer = _customerRepo.UpdateCustomer(id,customer);
            return GetBaseResult(CodeMessage._200, updatedCustomer);
        }

        /// <summary>
        /// Thêm mới một khách hàng vào hệ thống.
        /// </summary>
        /// <param name="customer">Đối tượng khách hàng cần thêm.</param>
        /// <returns>Kết quả chứa thông tin khách hàng sau khi thêm.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby
        public async Task<BaseResult<Customer>> InsertCustomer(Customer customer)
        {
            var insertedCustomer = _customerRepo.Insert(customer);
            return GetBaseResult(CodeMessage._200, insertedCustomer);
        }

        /// <summary>
        /// Xóa một khách hàng dựa theo mã khách hàng.
        /// </summary>
        /// <param name="customerCode">Mã khách hàng cần xóa.</param>
        /// <returns>Kết quả sau khi thực hiện xóa.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>
        public async Task<BaseResult<Customer>> DeleteCustomer(string customerCode)
        {
            var deletedCustomer = _customerRepo.RemoveCustomer(customerCode);
            return GetBaseResult(CodeMessage._200, deletedCustomer);
        }

        /// <summary>
        /// Lấy danh sách khách hàng theo mã khách hàng (hỗ trợ phân trang mặc định).
        /// </summary>
        /// <param name="customerCode">Mã khách hàng cần tìm.</param>
        /// <returns>Danh sách khách hàng phù hợp.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>
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

        /// <summary>
        /// Sinh mã khách hàng mới tự động theo quy tắc: KH + yyyyMM + số thứ tự 6 chữ số.
        /// </summary>
        /// <returns>Mã khách hàng mới.</returns>
        /// <example>KH202510000001</example>
        /// <createdby> NVTien - 28.10.2025 </createdby>
        public async Task<BaseResult<string>> GenerateCustomerCode()
        {
            string prefix = "KH" + DateTime.Now.ToString("yyyyMM");

            string latestCode = _customerRepo.GetLatestCustomerCode(prefix);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(latestCode) && latestCode.Length >= 14)
            {
                string lastNumberStr = latestCode.Substring(8); 
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            string newCode = prefix + nextNumber.ToString("D6");
            return GetBaseResult(CodeMessage._200, newCode);
        }

        /// <summary>
        /// Lọc danh sách khách hàng theo từ khóa (theo tên, mã, công ty,...) có phân trang.
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm.</param>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="pageSize">Số bản ghi mỗi trang.</param>
        /// <returns>Danh sách khách hàng thỏa mãn điều kiện lọc.</returns>
        /// <createdby> NVTien - 28.10.2025 </createdby>
        public async Task<PaginationResult<CustomerDto>> FilterCustomers(string? keyword, int page, int pageSize)
        {
        
            var customers = _customerRepo.FilterCustomers(keyword);
            var totalRecords = customers.Count;
            var pagedData = customers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<CustomerDto>(pagedData, page, pageSize, totalRecords);


        }

        public string GenerateCustomerCodeHelper()
        {
            string prefix = "KH" + DateTime.Now.ToString("yyyyMM");

            string latestCode = _customerRepo.GetLatestCustomerCode(prefix);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(latestCode) && latestCode.Length >= 14)
            {
                string lastNumberStr = latestCode.Substring(8);
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            string newCode = prefix + nextNumber.ToString("D6");
            return newCode;
        }

        /// <summary>
        /// Import danh sách khách hàng từ file Excel (.xlsx).
        /// </summary>
        /// <param name="stream">Dòng dữ liệu đọc từ file Excel được upload.</param>
        /// <returns>Danh sách khách hàng được import thành công.</returns>
        /// <remarks>
        /// Cột Excel được đọc theo thứ tự:
        /// 1. CustomerTypeId  
        /// 2. CustomerCode  
        /// 3. FullName  
        /// 4. CompanyName  
        /// 5. PhoneNumber  
        /// 6. TaxCode  
        /// </remarks>
        /// <createdby> NVTien - 28.10.2025 </createdby>
        public async Task<BaseResult<List<Customer>>> ImportCustomers(Stream stream)
        {
           

            var importedCustomers = new List<Customer>();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) 
                {
                    string fullName = worksheet.Cells[row, 3].Text?.Trim();
                    if (string.IsNullOrWhiteSpace(fullName))
                        continue; 

                    var customerDto = new ImportCustomer
                    {
                        CustomerTypeId = worksheet.Cells[row, 1].Text?.Trim(),
                        CustomerCode = GenerateCustomerCodeHelper(),
                        FullName = fullName,
                        CompanyName = worksheet.Cells[row, 3].Text?.Trim(),
                        PhoneNumber = worksheet.Cells[row, 4].Text?.Trim(),
                        TaxCode = worksheet.Cells[row, 5].Text?.Trim(),
                    };

                    var customer = Mapper.Map<Customer>(customerDto);

                    _customerRepo.Insert(customer);
                    importedCustomers.Add(customer);
                }
            }

            return GetBaseResult(CodeMessage._200, importedCustomers);
        }
    }
}
