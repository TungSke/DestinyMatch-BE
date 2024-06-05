using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models.Request
{
    public class MessageRequest
    {
        public string Content { get; set; } = null!;


        public string? Status { get; set; }

        public Guid? ConversationId { get; set; }

        public Guid? SenderId { get; set; }
    }
}
