namespace Repository.DTOs.Picture
{
    public class GetPicture
    {
        public Guid Id { get; set; }

        public string? UrlPath { get; set; }

        public bool? IsAvatar { get; set; }

        public Guid? MemberId { get; set; }
    }
}
