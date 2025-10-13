using Application.Features.AppBasic.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy.Models
{
    public class ModalDataResponse
    {
        public List<BusinessUnitResponse> BusinessUnits { get; set; }
        public List<BrandResponse> Brands { get; set; }
    }
}

