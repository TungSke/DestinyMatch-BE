namespace FPT.DestinyMatch.API.Models.ResponseModels
{
    public class PagedResultResponse<T> where T : class
    {
        public int TotalCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));

        public IEnumerable<T> ResultsList { get; set; } = [];

    }
}
