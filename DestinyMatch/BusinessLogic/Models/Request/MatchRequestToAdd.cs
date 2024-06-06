using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models.Request
{
    public class MatchRequestToAdd
    {

        public Guid FromId { get; set; }

        public Guid ToId { get; set; }
    }
}
