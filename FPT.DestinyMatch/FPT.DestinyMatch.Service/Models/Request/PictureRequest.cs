using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Models.Request
{
    public class PictureRequest
    {
        public Guid Id { get; set; }
        public bool? IsAvatar { get; set; }
        public Guid? MemberId { get; set; }
    }
}
