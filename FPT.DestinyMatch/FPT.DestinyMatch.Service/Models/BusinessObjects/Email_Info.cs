namespace FPT.DestinyMatch.Service.Models.BusinessObjects
{
    public class Email_Info
    {
        public required string SubjectContent { get; set; }

        public required string BodyContent { get; set; }

        public required string ReceiverEmail { get; set; }
    }
}
