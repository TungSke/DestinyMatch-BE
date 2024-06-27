using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Models.Request
{
    public class MatchingRequest
    {
        [JsonIgnore]
        public Guid thisMemberId { get; set; }

        public Guid toMemberId { get; set; }
    }
}
