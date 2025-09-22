using Application.Features.Instruments;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instruments.Responses;
using Domain.Entities;
using Application.Features.Customers.Requests;
//using Instrument = Domain.Entities.Instrument;

namespace Infrastructure.Services
{
    public class InstrumentService(ApplicationDbContext Context, ICurrentUserService currentUserService) : IInstrumentService
    {

        public async Task<Instrument> GetInstrumentEntityAsync(Guid id)
        {
            var instrument = await Context.Instrument.FirstOrDefaultAsync(x => x.Id == id);
            //if (instrument != null)
            //{
            //    instrument.InsMfgDt = Convert.ToDateTime(instrument.InsMfgDt).ToString("dd/MM/yyyy");
            //    //instrument.Image = getImage(instrument.Image);
            //}
            return instrument;
        }

        public async Task<InstrumentResponse> GetInstrumentAsync(Guid id)
        {
            var instrument = await (from i in Context.Instrument
                                    join b in Context.BusinessUnit on i.BusinessUnitId equals b.Id
                                    join br in Context.Brand on i.BrandId equals br.Id
                                    join m in Context.Manufacturer on i.ManufId equals m.Id
                                    join it in Context.ListTypeItems on i.InsType equals it.Id.ToString()
                                    where i.Id == id
                                    select new InstrumentResponse
                                    {
                                        Id = i.Id,
                                        InsTypeName = it.ItemName,
                                        BrandId = br.Id,
                                        BrandName = br.BrandName,
                                        BusinessUnitId = b.Id,
                                        BusinessUnitName = b.BusinessUnitName,
                                        Image = i.Image,
                                        InsMfgDt = i.InsMfgDt,// Convert.ToDateTime(i.InsMfgDt).ToString("dd/MM/yyyy"),
                                        InsType = i.InsType,
                                        InsVersion = i.InsVersion,
                                        IsActive = i.IsActive,
                                        IsDeleted = i.IsDeleted,
                                        ManufId = i.ManufId,
                                        ManufName = m.ManfName,
                                        SerialNos = i.SerialNos,
                                        CreatedBy = i.CreatedBy,
                                        CreatedOn = i.CreatedOn
                                    }).FirstOrDefaultAsync();

            //if (instrument != null)
            //    instrument.Image = getImage(instrument.Image);

            return instrument;
        }

        public async Task<InstrumentResponse> GetInstrumentBySerialNoAsync(string serialNo)
        {
            var instrument = await (from i in Context.Instrument
                                    join b in Context.BusinessUnit on i.BusinessUnitId equals b.Id
                                    join br in Context.Brand on i.BrandId equals br.Id
                                    join m in Context.Manufacturer on i.ManufId equals m.Id
                                    join it in Context.ListTypeItems on i.InsType equals it.Id.ToString()
                                    where i.SerialNos == serialNo
                                    select new InstrumentResponse
                                    {
                                        Id = i.Id,
                                        InsTypeName = it.ItemName,
                                        BrandId = br.Id,
                                        BrandName = br.BrandName,
                                        BusinessUnitId = b.Id,
                                        BusinessUnitName = b.BusinessUnitName,
                                        Image = i.Image,
                                        InsMfgDt = i.InsMfgDt, //Convert.ToDateTime(i.InsMfgDt).ToString("dd/MM/yyyy"),
                                        InsType = i.InsType,
                                        InsVersion = i.InsVersion,
                                        IsActive = i.IsActive,
                                        IsDeleted = i.IsDeleted,
                                        ManufId = i.ManufId,
                                        ManufName = m.ManfName,
                                        SerialNos = i.SerialNos,
                                        CreatedBy = i.CreatedBy,
                                        CreatedOn = i.CreatedOn
                                    }).FirstOrDefaultAsync();

            //if(instrument != null)
            //    instrument.Image = getImage(instrument.Image);

            return instrument;
        }



        private string getImage(string image)
        {
            string img = string.Empty;
            if (!string.IsNullOrEmpty(image))
            {
                var fileName = Path.Combine(Directory.GetCurrentDirectory(), image);
                if (File.Exists(fileName))
                {
                    var bytes = File.ReadAllBytes(fileName);
                    var file = Convert.ToBase64String(bytes);
                    img = file;
                }
            }
            return img;
        }

        public async Task<List<InstrumentResponse>> GetInstrumentsAsync(string businessUnitId, string brandId)
        {
            var instruments = await (from i in Context.Instrument
                                     join b in Context.BusinessUnit on i.BusinessUnitId equals b.Id
                                     join br in Context.Brand on i.BrandId equals br.Id
                                     join m in Context.Manufacturer on i.ManufId equals m.Id
                                     join it in Context.ListTypeItems on i.InsType equals it.Id.ToString()
                                     select new InstrumentResponse
                                     {
                                         Id = i.Id,
                                         InsTypeName = it.ItemName,
                                         BrandId = br.Id,
                                         BrandName = br.BrandName,
                                         BusinessUnitId = b.Id,
                                         BusinessUnitName = b.BusinessUnitName,
                                         Image = i.Image,
                                         InsMfgDt = i.InsMfgDt, // Convert.ToDateTime(i.InsMfgDt).ToString("dd/MM/yyyy"),
                                         InsType = i.InsType,
                                         InsVersion = i.InsVersion,
                                         IsActive = i.IsActive,
                                         IsDeleted = i.IsDeleted,
                                         ManufId = i.ManufId,
                                         ManufName = m.ManfName,
                                         SerialNos = i.SerialNos,
                                         CreatedBy = i.CreatedBy,
                                         CreatedOn = i.CreatedOn
                                     }).ToListAsync();

            if (!string.IsNullOrEmpty(businessUnitId))
                instruments = instruments.Where(x => x.BusinessUnitId.ToString() == businessUnitId && x.BrandId.ToString() == brandId).ToList();

            return instruments;
        }

        public async Task<Guid> CreateInstrumentAsync(Domain.Entities.Instrument Instrument)
        {
            if (!Context.Instrument.Any(x => x.InsType == Instrument.InsType && x.InsVersion == Instrument.InsVersion 
            && x.SerialNos == Instrument.SerialNos && x.ManufId == Instrument.ManufId))
            {
                Instrument.CreatedOn = DateTime.Now;
                Instrument.UpdatedOn = DateTime.Now;
                Instrument.CreatedBy = Guid.Parse(currentUserService.GetUserId());
                Instrument.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

                await Context.Instrument.AddAsync(Instrument);
                await Context.SaveChangesAsync();
            }
            return Instrument.Id;
        }

        public async Task<bool> DeleteInstrumentAsync(Guid id)
        {

            var deletedInstrument = await Context.Instrument.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedInstrument == null) return true;

            //deletedInstrument.IsDeleted = true;
            //deletedInstrument.IsActive = false;

            Context.Entry(deletedInstrument).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateInstrumentAsync(Domain.Entities.Instrument Instrument)
        {
            Instrument.UpdatedOn = DateTime.Now;
            Instrument.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            Context.Entry(Instrument).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return Instrument.Id;
        }

        public async Task<bool> IsDuplicateAsync(string insType, string serialNos)
            => await Context.Instrument.AnyAsync(x => x.InsType.ToUpper() == insType.ToUpper() && x.SerialNos.ToUpper() == serialNos.ToUpper());
    }
}