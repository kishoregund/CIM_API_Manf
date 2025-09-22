using Application.Features.Schools;
using Application.Features.Spares;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Domain.Views;
using Application.Features.Spares.Responses;

namespace Infrastructure.Services
{
    public class SparepartService(ApplicationDbContext _context, ICurrentUserService currentUserService) : ISparepartService
    {
        private readonly ApplicationDbContext _context = _context;

        public Task<Sparepart> GetSparepartEntityAsync(Guid id)
            => _context.Spareparts.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<VW_Spareparts> GetSparepartAsync(Guid id)
        {
            var sparepart = await _context.VW_Spareparts.FirstOrDefaultAsync(x => x.Id == id);
            if (!string.IsNullOrEmpty(sparepart.Image))
            {
                var fileName = Path.Combine(Directory.GetCurrentDirectory(), sparepart.Image);
                if (File.Exists(fileName))
                {
                    var bytes = File.ReadAllBytes(fileName);
                    var file = Convert.ToBase64String(bytes);
                    sparepart.Image = file;
                }
            }

            return sparepart;
        }

        public async Task<List<VW_Spareparts>> GetSparepartsAsync()
           => await _context.VW_Spareparts.ToListAsync();

        public async Task<List<VW_Spareparts>> GetConfigSparepartAsync(Guid configTypeId)//, Guid configValueId)
            => await _context.VW_Spareparts.Where(x => x.ConfigTypeId == configTypeId).OrderBy(x=>x.PartNo).ToListAsync();

        public async Task<VW_Spareparts> GetSparepartByPartNoAsync(string partNo)
            => await _context.VW_Spareparts.FirstOrDefaultAsync(x => x.PartNo == partNo);

        public async Task<Guid> CreateSparepartAsync(Sparepart Sparepart)
        {
            Sparepart.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            Sparepart.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Sparepart.CreatedOn = DateTime.Now;
            Sparepart.UpdatedOn = DateTime.Now;
            Sparepart.IsActive = true;
            Sparepart.IsDeleted = false;

            await _context.Spareparts.AddAsync(Sparepart);
            await _context.SaveChangesAsync();
            return Sparepart.Id;
        }

        public async Task<bool> DeleteSparepartAsync(Guid id)
        {
            var deletedSparepart = await _context
                .Spareparts.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedSparepart == null) return true;

            deletedSparepart.IsDeleted = true;
            deletedSparepart.IsActive = false;

            _context.Entry(deletedSparepart).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSparepartAsync(Sparepart Sparepart)
        {
            Sparepart.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Sparepart.UpdatedOn = DateTime.Now;

            _context.Entry(Sparepart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Sparepart.Id;
        }

    }
}
