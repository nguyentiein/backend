using AutoMapper;
using Microsoft.Extensions.Options;
using SalesManagement.BusinessLogic.Result;
using SalesManagement.BusinessLogic.Result.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SalesManagement.BusinessLogic.Services
{
    public class BaseService
    {
        #region Property
        protected readonly IMapper Mapper;
        protected readonly ResponseMessage ResponseMessage;
        #endregion

        #region Constructor
        public BaseService(IMapper mapper,
            IOptionsMonitor<ResponseMessage> responseMessage)
        {
            Mapper = mapper;
            ResponseMessage = responseMessage.CurrentValue;
        }
        #endregion

        #region Method
        public virtual BaseResult<Inner> GetBaseResult<Inner>(
      CodeMessage statusCode,
      Inner data = default,
      StatusEnum status = StatusEnum.Success,
      string message = "",
      MetaData meta = null)
        {
            string nameStatusCode = statusCode.GetElementNameCodeMessage();

            string tempCode = string.IsNullOrEmpty(nameStatusCode) ? "217" : nameStatusCode.RemoveSpaceCharacter();
            string tempMessage = string.IsNullOrEmpty(message) || message.Length > 500
                ? ResponseMessage.Values[tempCode].RemoveSpaceCharacter()
                : message.RemoveSpaceCharacter();

            // Nếu status là Success → trả về data + meta
            if (status == StatusEnum.Success)
            {
                return new BaseResult<Inner>()
                {
                    Status = status,
                    Data = data,
                    Meta = meta,
                    Error = null
                };
            }

            // Nếu status là Failed → trả về error
            return new BaseResult<Inner>()
            {
                Status = status,
                Data = default,
                Meta = null,
                Error = new ErrorData
                {
                    Code = tempCode,
                    Message = tempMessage
                }
            };
        }

        #endregion

    }
}

