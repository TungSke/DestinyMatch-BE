using Newtonsoft.Json;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class AccountFilter
    {
        public int Amount { get; set; }
        public int Page { get; set; }
        public string? EmailKeyword { get; set; }
        public bool ByDate { get; set; } = true;
        public string? Status { get; set; }
        public string? Role { get; set; }
        public bool OrderByDescending { get; set; } = true;
    }
}
