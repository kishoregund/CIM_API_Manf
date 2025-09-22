using Application.Features.Customers;
using Application.Features.Customers.Responses;
using Application.Features.Identity.Roles;
using Application.Features.Identity.Users;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CustomerSurveyService(ApplicationDbContext Context, ICurrentUserService currentUserService) : ICustomerSurveyService
    {
        public async Task<Guid> CreateCustomerSurveyAsync(CustomerSatisfactionSurvey CustomerSurvey)
        {
            CustomerSurvey.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            CustomerSurvey.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            CustomerSurvey.CreatedOn = DateTime.Now;
            CustomerSurvey.UpdatedOn = DateTime.Now;
            CustomerSurvey.IsActive = true;
            CustomerSurvey.IsDeleted = false;

            await Context.CustomerSatisfactionSurvey.AddAsync(CustomerSurvey);
            await Context.SaveChangesAsync();

            return CustomerSurvey.Id;
        }

        public async Task<bool> DeleteCustomerSurveyAsync(Guid id)
        {
            var deletedCustomer = await Context.CustomerSatisfactionSurvey.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deletedCustomer != null)
            {
                deletedCustomer.IsDeleted = true;
                deletedCustomer.IsActive = false;

                Context.Entry(deletedCustomer).State = EntityState.Deleted;
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<CustomerSatisfactionSurvey> GetCustomerSurveyAsync(Guid id)
            => await Context.CustomerSatisfactionSurvey.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<CustomerSurveyResponse>> GetCustomerSurveysAsync()
        {
            var surveys = await ( from s in Context.CustomerSatisfactionSurvey
                                  join c in Context.RegionContact on s.EngineerId equals c.Id
                                  join sr in Context.ServiceRequest on s.ServiceRequestId equals sr.Id
                                  select new CustomerSurveyResponse
                                  {
                                      Comments = s.Comments,
                                      Id = s.Id,
                                      ServiceRequestId = s.ServiceRequestId,
                                      CreatedBy = s.CreatedBy,
                                      CreatedOn = s.CreatedOn,
                                      DistId = s.DistId,
                                      Email = s.Email,
                                      EngineerId = s.EngineerId,
                                      EngineerName = c.FirstName + " " + c.LastName,
                                      IsActive = s.IsActive,
                                      IsAreaClean = s.IsAreaClean,
                                      IsDeleted = s.IsDeleted,
                                      IsNote = s.IsNote,
                                      IsNotified = s.IsNotified,
                                      IsProfessional = s.IsProfessional,
                                      IsSatisfied = s.IsAreaClean,
                                      Name = s.Name,
                                      OnTime = s.OnTime,
                                      ServiceRequestNo = sr.SerReqNo,
                                      UpdatedBy = s.UpdatedBy,
                                      UpdatedOn = s.UpdatedOn
                                  }).ToListAsync();
            return surveys;
        }

        public async Task<Guid> UpdateCustomerSurveyAsync(CustomerSatisfactionSurvey CustomerSurvey)
        {
            CustomerSurvey.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            CustomerSurvey.UpdatedOn = DateTime.Now;
            Context.Entry(CustomerSurvey).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return CustomerSurvey.Id;
        }
    }
}
