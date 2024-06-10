using Newtonsoft.Json;

namespace FPT.DestinyMatch.API.Models.ResponseModels
{
    public class AuthenticationAccount
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("role")] public string Role { get; set; }
    }
}
