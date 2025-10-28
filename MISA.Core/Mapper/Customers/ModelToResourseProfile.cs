using AutoMapper;
using SalesManagement.BusinessLogic.Core.Entities;
using SalesManagement.BusinessLogic.Dtos.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Mapper.Customers
{
    public  class ModelToResourseProfile:Profile
    {
        public ModelToResourseProfile() {

     CreateMap<CreateCustomer, Customer>()
    .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
    .ForMember(dest => dest.CustomerTypeId, opt => opt.MapFrom(src => src.CustomerType))
    .ForMember(dest => dest.CustomerType, opt => opt.Ignore());


        }
    }
}
