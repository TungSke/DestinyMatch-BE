using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class GuidRequestor
    {
        [Required]
        public Guid Id { get; set; }
    }
}
