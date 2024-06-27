using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class GoogleResponse
    {
        [Required] required public string Token { get; set; }
        [Required] required public string Platform {  get; set; }
    }
}
