using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Models.Response
{
    public class PictureResponse
    {
        public Guid Id { get; set; }
        public string? UrlPath { get; set; }
        public bool? IsAvatar { get; set; }
        public Guid? MemberId { get; set; }
    }
}
