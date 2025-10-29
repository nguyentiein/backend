using Microsoft.Extensions.Configuration;
using SalesManagement.BusinessLogic.Core.Entities;
using SalesManagement.BusinessLogic.Entities;
using SalesManagement.BusinessLogic.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.DataAccess.Repositories
{
    public class CustomerTypeRepo : BaseRepo<CustomerType>, ICustomerTypeRepo
    {
        public CustomerTypeRepo(IConfiguration config) : base(config)
        {

        }
    
    }
}
