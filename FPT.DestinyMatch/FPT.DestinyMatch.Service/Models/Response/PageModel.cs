namespace FPT.DestinyMatch.API.Models.ResponseModels
{
    public class PageModel<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int totalPage { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
