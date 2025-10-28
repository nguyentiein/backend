using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Result
{
    public class BaseResult<T>
    {
        #region Properties

        [JsonPropertyName("data")]
        public T Data { get; init; }

        [JsonPropertyName("meta")]
        public MetaData Meta { get; init; }

        [JsonPropertyName("error")]
        public ErrorData Error { get; init; }

        [JsonIgnore]
        public StatusEnum Status { get; init; }

        #endregion

        #region Constructors

        public BaseResult() { }

        public BaseResult(T data, MetaData meta = null)
        {
            Data = data;
            Meta = meta;
            Status = StatusEnum.Success;
            Error = null;
        }

        public BaseResult(string message, string code = null)
        {
            Data = default;
            Meta = null;
            Status = StatusEnum.Failed;
            Error = new ErrorData { Message = message, Code = code };
        }

        #endregion
    }

    public class MetaData
    {
        [JsonPropertyName("page")]
        public int Page { get; init; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; init; }

        [JsonPropertyName("total")]
        public int Total { get; init; }
    }

    public class ErrorData
    {
        [JsonPropertyName("code")]
        public string Code { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; }
    }
}
