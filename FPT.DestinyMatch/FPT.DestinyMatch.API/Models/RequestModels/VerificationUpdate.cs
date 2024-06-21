using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class VerificationUpdate
    {
        [Required] public Guid Id { get; set; }
        [Required] public string Status { get; set; }
    }
}
