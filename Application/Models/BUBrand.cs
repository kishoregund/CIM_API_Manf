using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class BUBrand
    {
        [SkipGlobalValidation]
        public string BusinessUnitId { get; set; }
        [SkipGlobalValidation]
        public string BrandId { get; set; }
    }
}
