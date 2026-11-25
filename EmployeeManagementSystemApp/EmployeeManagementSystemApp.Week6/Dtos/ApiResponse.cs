namespace EmployeeManagementSystemApp.Week6.Dtos
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public IReadOnlyList<string>? Errors { get; set; }
        public string? TraceId { get; set; }

        public static ApiResponse<T> Ok(T data, string message, string? traceId = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TraceId = traceId
            };
        }

        public static ApiResponse<T> Fail(string message, IReadOnlyList<string>? errors = null, string? traceId = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                TraceId = traceId
            };
        }
    }

    public class PaginationMeta
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

    public class PagedResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public IReadOnlyList<T> Data { get; set; } = Array.Empty<T>();
        public PaginationMeta Pagination { get; set; } = new PaginationMeta();
        public string? TraceId { get; set; }
    }
}
