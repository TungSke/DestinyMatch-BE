using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class MessageReposirory : GenericRepository<Message>, IMessageReposirory
    {
        public MessageReposirory(DestinyMatchContext _dbcontext) : base(_dbcontext)
        {
        }
    }
}
