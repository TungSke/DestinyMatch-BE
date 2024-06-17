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
        [Required(ErrorMessage = "Picture Id require")]
        public Guid PictureId { get; set; }
        [Required(ErrorMessage = "Picture URL is require")]
        public required string UrlPath { get; set; }
    }
}
