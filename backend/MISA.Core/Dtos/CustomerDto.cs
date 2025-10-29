using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Dtos
{
    public class CustomerDto
    {
        public string ? CustomerType { get; set; }
        public string? CustomerCode { get; set; }
        public string ?FullName { get; set; }
        public string? CompanyName { get; set; }

        public string ?ShippingAddresses { get; set; }
        public string ?PhoneNumber { get; set; }
        public DateTime? LatestPurchaseDate { get; set; }
        public string? PurchasedProductCodes { get; set; }
        public string ?PurchasedProductNames { get; set; }
    }
}
