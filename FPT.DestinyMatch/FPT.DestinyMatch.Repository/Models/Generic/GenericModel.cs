using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Repository.Models.Generic
{
    public interface GenericModel<Tkey>
    {
        Tkey Id { get; }
    }
}
