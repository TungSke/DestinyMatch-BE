using BusinessLogic.Models.Request;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessages();

        Task<Message?> GetMessageById(Guid id);
        Task<bool> DeleteMessage(Guid memberId);
        Task<Message> CreateMessage(MessageRequest messageRequest);
        Task<Message> UpdateMessage(Guid Id, MessageRequest messageRequest);
    }
}
