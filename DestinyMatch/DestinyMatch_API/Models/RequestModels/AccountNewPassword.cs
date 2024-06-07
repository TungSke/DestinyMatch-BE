namespace DestinyMatch_API.Models.RequestModels
{
    public class AccountNewPassword
    {
        public Guid Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
