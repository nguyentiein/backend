using SalesManagement.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  SalesManagement.BusinessLogic.Core.Entities
{
    public class Customer
    {
        public string ?CustomerId { get; set; }
        public string ?CustomerCode { get; set; }        
        public string? CustomerTypeId { get; set; }      
        public string ?FullName { get; set; }           
        public string? CompanyName { get; set; }      
        public string ?TaxCode { get; set; }

        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Phone number must be 10 or 11 digits.")]
        public string ?PhoneNumber { get; set; }
        public string ?Address { get; set; }             
        public string ?Email { get; set; }            
        public DateTime? LatestPurchaseDate { get; set; }
        public decimal DebtAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ? CustomerTypeName { get; set; }
    }
}
