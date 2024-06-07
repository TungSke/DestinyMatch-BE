using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOs.University
{
    public class UpdateUni
    {
        public Guid Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }
    }
}
