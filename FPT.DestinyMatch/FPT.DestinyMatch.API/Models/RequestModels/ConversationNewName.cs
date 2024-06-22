using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class ConversationNewName
    {
        [Required] public Guid ConversationId { get; set; }
        [Required] public required string NewName { get; set; }
    }
}
