using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ManfBusinessUnit : BaseEntity
    {
        public string ManfId { get; set; }
        public string BusinessUnitName { get; set; }
    }
}
