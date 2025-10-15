using Application.Features.ServiceRequests.Commands;
using Application.Features.ServiceRequests;
using Domain.Entities;
using Application.Features.ServiceRequests.Requests;

namespace Application.Features.ServiceRequests.Commands
{
    public class UpdateServiceRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public ServiceRequestRequest ServiceRequestRequest { get; set; }
    }

    public class UpdateServiceRequestCommandHandler(IServiceRequestService ServiceRequestService) : IRequestHandler<UpdateServiceRequestCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateServiceRequestCommand request, CancellationToken cancellationToken)
        {
            var ServiceRequestInDb = await ServiceRequestService.GetServiceRequestEntityAsync(request.ServiceRequestRequest.Id);


            ServiceRequestInDb.Id = request.ServiceRequestRequest.Id;
            ServiceRequestInDb.Accepted = request.ServiceRequestRequest.Accepted;
            ServiceRequestInDb.AcceptedDate = request.ServiceRequestRequest.AcceptedDate;
            ServiceRequestInDb.AlarmDetails = request.ServiceRequestRequest.AlarmDetails;
            ServiceRequestInDb.AmcId = request.ServiceRequestRequest.AmcId;
            ServiceRequestInDb.AmcServiceQuote = request.ServiceRequestRequest.AmcServiceQuote;
            ServiceRequestInDb.BreakoccurDetailsId = request.ServiceRequestRequest.BreakoccurDetailsId;
            ServiceRequestInDb.AssignedTo = request.ServiceRequestRequest.AssignedTo;
            ServiceRequestInDb.BaseAmt = request.ServiceRequestRequest.BaseAmt;
            ServiceRequestInDb.BaseCurrency = request.ServiceRequestRequest.BaseCurrency;
            ServiceRequestInDb.BreakdownType = request.ServiceRequestRequest.BreakdownType;
            ServiceRequestInDb.CompanyName = request.ServiceRequestRequest.CompanyName;
            ServiceRequestInDb.ComplaintRegisName = request.ServiceRequestRequest.ComplaintRegisName;
            ServiceRequestInDb.ContactPerson = request.ServiceRequestRequest.ContactPerson;
            ServiceRequestInDb.CostInUsd = request.ServiceRequestRequest.CostInUsd;
            ServiceRequestInDb.Country = request.ServiceRequestRequest.Country;
            ServiceRequestInDb.CurrentInstrustatus = request.ServiceRequestRequest.CurrentInstrustatus;
            ServiceRequestInDb.CustId = request.ServiceRequestRequest.CustId;
            ServiceRequestInDb.DelayedReasons = request.ServiceRequestRequest.DelayedReasons;
            ServiceRequestInDb.DistId = request.ServiceRequestRequest.DistId;
            ServiceRequestInDb.Distributor = request.ServiceRequestRequest.Distributor;
            ServiceRequestInDb.EDate = request.ServiceRequestRequest.EDate;
            ServiceRequestInDb.Email = request.ServiceRequestRequest.Email;
            ServiceRequestInDb.Escalation = request.ServiceRequestRequest.Escalation;
            ServiceRequestInDb.IsCritical = request.ServiceRequestRequest.IsCritical;
            //ServiceRequestInDb.IsNotUnderAmc = request.ServiceRequestRequest.IsNotUnderAmc; // should not be updated as the its checked on create
            ServiceRequestInDb.IsRecurring = request.ServiceRequestRequest.IsRecurring;
            ServiceRequestInDb.IsReportGenerated = request.ServiceRequestRequest.IsReportGenerated;
            ServiceRequestInDb.MachEngineer = request.ServiceRequestRequest.MachEngineer;
            ServiceRequestInDb.MachinesNo = request.ServiceRequestRequest.MachinesNo;
            ServiceRequestInDb.MachmodelName = request.ServiceRequestRequest.MachmodelName;
            ServiceRequestInDb.OperatorEmail = request.ServiceRequestRequest.OperatorEmail;
            ServiceRequestInDb.OperatorName = request.ServiceRequestRequest.OperatorName;
            ServiceRequestInDb.OperatorNumber = request.ServiceRequestRequest.OperatorNumber;
            ServiceRequestInDb.RecurringComments = request.ServiceRequestRequest.RecurringComments;
            ServiceRequestInDb.RegistrarPhone = request.ServiceRequestRequest.RegistrarPhone;
            ServiceRequestInDb.Remarks = request.ServiceRequestRequest.Remarks;
            ServiceRequestInDb.RequestTime = request.ServiceRequestRequest.RequestTime;
            ServiceRequestInDb.RequestTypeId = request.ServiceRequestRequest.RequestTypeId;
            ServiceRequestInDb.ResolveAction = request.ServiceRequestRequest.ResolveAction;
            ServiceRequestInDb.SampleHandlingType = request.ServiceRequestRequest.SampleHandlingType;
            ServiceRequestInDb.SDate = request.ServiceRequestRequest.SDate;
            ServiceRequestInDb.SerReqDate = request.ServiceRequestRequest.SerReqDate;
            ServiceRequestInDb.SerReqNo = request.ServiceRequestRequest.SerReqNo;
            ServiceRequestInDb.SerResolutionDate = request.ServiceRequestRequest.SerResolutionDate;
            ServiceRequestInDb.SiteId = request.ServiceRequestRequest.SiteId;
            ServiceRequestInDb.SiteName = request.ServiceRequestRequest.SiteName;
            ServiceRequestInDb.StageId = request.ServiceRequestRequest.StageId;
            ServiceRequestInDb.StatusId = request.ServiceRequestRequest.StatusId;
            ServiceRequestInDb.SiteUserId = request.ServiceRequestRequest.SiteUserId;
            ServiceRequestInDb.SubRequestTypeId = request.ServiceRequestRequest.SubRequestTypeId;
            ServiceRequestInDb.TotalCost = request.ServiceRequestRequest.TotalCost;
            ServiceRequestInDb.TotalCostCurrency = request.ServiceRequestRequest.TotalCostCurrency;
            ServiceRequestInDb.VisitType = request.ServiceRequestRequest.VisitType;
            ServiceRequestInDb.XrayGenerator = request.ServiceRequestRequest.XrayGenerator;
            ServiceRequestInDb.UpdatedBy = request.ServiceRequestRequest.UpdatedBy;

            var updateServiceRequestId = await ServiceRequestService.UpdateServiceRequestAsync(ServiceRequestInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateServiceRequestId, message: "Record updated successfully.");
        }
    }
}
