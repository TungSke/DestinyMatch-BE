using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Models.Request
{
    public class PackageRequest
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Price { get; set; }

        public string? Status { get; set; }
    }
}
