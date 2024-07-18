namespace FPT.DestinyMatch.API.Models.RequestModels.Paging
{
    public class MemberPaging
    {
        public int Amount { get; set; } = 5;
        public int Page { get; set; } = 1;
        public string? EmailKeyword { get; set; }
        public string? NameKeyword { get; set; }
        public bool? Gender { get; set; }
        public string? Status { get; set; }
        public string? UniversityKeyword { get; set; }
        public string? MajorKeyword { get; set; }
        public List<string>? HobbyList { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public bool OrderByName_Descending { get; set; } = true;
    }
}
