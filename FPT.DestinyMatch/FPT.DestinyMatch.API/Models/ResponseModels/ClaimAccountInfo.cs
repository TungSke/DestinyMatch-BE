using Newtonsoft.Json;

namespace FPT.DestinyMatch.API.Models.ResponseModels
{
    public class ClaimAccountInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string MemberId { get; set; }
    }
}
