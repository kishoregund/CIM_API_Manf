using Application.Features.AppBasic;
using Application.Features.Identity.Users;
using Application.Features.Tenancy.Models;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AppBasicService(ApplicationDbContext context, ICurrentUserService currentUserService) : IAppBasicService
    {
        public async Task<ModalDataResponse> GetModalDataAsync()
        {
            BrandService brandService = new(context, currentUserService);
            BusinessUnitService businessUnitService = new(context, currentUserService);

            var resp =  new ModalDataResponse
            {
                BusinessUnits = await businessUnitService.GetBusinessUnitsAsync(),
                Brands =  await brandService.GetBrandsAsync(),
            };

            return resp;
        }
                        
    }
}
