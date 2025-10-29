using SalesManagement.BusinessLogic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Entities
{
    public  class CustomerPurchase
    {
        public string ?PurchaseId { get; set; }      
        public string ?CustomerId { get; set; }     
        public DateTime ?PurchaseDate { get; set; }
        public decimal ?TotalAmount { get; set; }
        public DateTime ?CreatedDate { get; set; }
        public Customer? Customer { get; set; }

    }
}
