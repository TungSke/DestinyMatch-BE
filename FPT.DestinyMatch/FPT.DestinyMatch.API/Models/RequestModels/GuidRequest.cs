using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class GuidRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
