namespace FPT.DestinyMatch.API.Models.ResponseModels
{
    public class PageModel<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
