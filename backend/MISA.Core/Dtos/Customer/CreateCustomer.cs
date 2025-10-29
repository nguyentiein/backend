using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Dtos.Customer
{
    public class CreateCustomer
    {
        public string ? CustomerType { get; set; }
        public string? CustomerCode { get; set; }
        [StringLength(128, ErrorMessage = "FullName cannot exceed 128 characters.")]
        public string? FullName { get; set; }
        public string? CompanyName { get; set; }
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Phone number must be 10 or 11 digits.")]
        public string ?PhoneNumber { get; set; }

        public string? TaxCode { get; set; }
    }
}
