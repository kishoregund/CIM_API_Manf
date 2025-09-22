using Application.Features.Customers.Commands;
using Application.Features.Customers;
using Application.Features.Customers.Requests;
using Domain.Entities;
using System.IO;

namespace Application.Features.Customers.Commands
{
    public class UpdateCustomerSurveyCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CustomerSurveyRequest CustomerSurveyRequest { get; set; }
    }

    public class UpdateCustomerSurveyCommandHandler(ICustomerSurveyService CustomerSurveyService) : IRequestHandler<UpdateCustomerSurveyCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateCustomerSurveyCommand request, CancellationToken cancellationToken)
        {
            var CustomerInDb = await CustomerSurveyService.GetCustomerSurveyAsync(request.CustomerSurveyRequest.Id);

            CustomerInDb.Id = request.CustomerSurveyRequest.Id;
            CustomerInDb.Comments = request.CustomerSurveyRequest.Comments;
            CustomerInDb.DistId = request.CustomerSurveyRequest.DistId;
            CustomerInDb.Email = request.CustomerSurveyRequest.Email;
            CustomerInDb.EngineerId= request.CustomerSurveyRequest.EngineerId;
            CustomerInDb.EngineerName = request.CustomerSurveyRequest.EngineerName;
            CustomerInDb.IsAreaClean = request.CustomerSurveyRequest.IsAreaClean;
            CustomerInDb.IsNote = request.CustomerSurveyRequest.IsNote;
            CustomerInDb.IsNotified = request.CustomerSurveyRequest.IsNotified;
            CustomerInDb.IsProfessional = request.CustomerSurveyRequest.IsProfessional;
            CustomerInDb.IsSatisfied = request.CustomerSurveyRequest.IsSatisfied;
            CustomerInDb.Name = request.CustomerSurveyRequest.Name;
            CustomerInDb.OnTime = request.CustomerSurveyRequest.OnTime;
            CustomerInDb.ServiceRequestId = request.CustomerSurveyRequest.ServiceRequestId;
            CustomerInDb.ServiceRequestNo = request.CustomerSurveyRequest.ServiceRequestNo;

            var updateCustomerId = await CustomerSurveyService.UpdateCustomerSurveyAsync(CustomerInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateCustomerId, message: "Record updated successfully.");
        }
    }
}
