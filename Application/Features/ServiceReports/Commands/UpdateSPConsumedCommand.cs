using Application.Features.ServiceReports.Requests;
using Domain.Entities;
using System;

namespace Application.Features.ServiceReports.Commands
{
    public class UpdateSPConsumedCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SPConsumedRequest SPConsumedRequest { get; set; }
    }

    public class UpdateSPConsumedCommandHandler(ISPConsumedService SPConsumedService) : IRequestHandler<UpdateSPConsumedCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSPConsumedCommand request, CancellationToken cancellationToken)
        {
            var SPConsumedInDb = await SPConsumedService.GetSPConsumedAsync(request.SPConsumedRequest.Id);


            SPConsumedInDb.Id = request.SPConsumedRequest.Id;
            SPConsumedInDb.ConfigType = request.SPConsumedRequest.ConfigType;
            SPConsumedInDb.ConfigValue = request.SPConsumedRequest.ConfigValue;
            SPConsumedInDb.CustomerSPInventoryId = request.SPConsumedRequest.CustomerSPInventoryId;
            SPConsumedInDb.HscCode = request.SPConsumedRequest.HscCode;
            SPConsumedInDb.PartNo = request.SPConsumedRequest.PartNo;
            SPConsumedInDb.QtyAvailable = request.SPConsumedRequest.QtyAvailable;
            SPConsumedInDb.QtyConsumed = request.SPConsumedRequest.QtyConsumed;
            SPConsumedInDb.ServiceReportId = request.SPConsumedRequest.ServiceReportId;
            SPConsumedInDb.UpdatedBy = request.SPConsumedRequest.UpdatedBy;


            var updateSPConsumedId = await SPConsumedService.UpdateSPConsumedAsync(SPConsumedInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSPConsumedId, message: "Record updated successfully.");
        }
    }
}
