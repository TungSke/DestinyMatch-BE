namespace DestinyMatch_API.Models.RequestModels
{
    public class AccountRecover
    {
        public string Email {  get; set; }
        public string Status {  get; set; }//Member = "experienced"       //Staff = "working"
    }
}
