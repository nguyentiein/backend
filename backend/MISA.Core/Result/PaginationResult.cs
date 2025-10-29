using System.Text.Json.Serialization;

namespace SalesManagement.BusinessLogic.Result
{
    public sealed class PaginationResult<T> : BaseResult<List<T>>
    {
        [JsonPropertyName("meta")]
        public new PaginationMeta Meta { get; init; }

        public PaginationResult(List<Dtos.CustomerDto> pagedData) : base() { }

        public PaginationResult(List<T> data, int page, int pageSize, int totalRecords)
        {
            Data = data;
            Meta = new PaginationMeta(page, pageSize, totalRecords);
            Status = StatusEnum.Success;
            Error = null;
        }

        public PaginationResult(string message, string code = "400") : base(message)
        {
            Error = new ErrorData { Message = message, Code = code };
        }
    }

    public class PaginationMeta : MetaData
    {
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; init; }

        [JsonPropertyName("totalRecords")]
        public int TotalRecords { get; init; }

        [JsonPropertyName("nextPage")]
        public int? NextPage { get; init; }

        [JsonPropertyName("previousPage")]
        public int? PreviousPage { get; init; }

        public PaginationMeta(int page, int pageSize, int totalRecords)
        {
            Page = page;
            PageSize = pageSize;
            Total = totalRecords;

            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            PreviousPage = page > 1 ? page - 1 : null;
            NextPage = page < TotalPages ? page + 1 : null;
        }
    }
}
