using Newtonsoft.Json;

namespace FPT.DestinyMatch.API.Models.ResponseModels
{
    public class JwtToken
    {
        [JsonProperty("token")] public string Token { get; set; }
    }
}
