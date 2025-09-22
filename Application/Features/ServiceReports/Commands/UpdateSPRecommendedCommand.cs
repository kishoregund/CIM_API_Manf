using Application.Features.ServiceReports.Requests;

namespace Application.Features.ServiceReports.Commands
{
    public class UpdateSPRecommendedCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SPRecommendedRequest SPRecommendedRequest { get; set; }
    }

    public class UpdateSPRecommendedCommandHandler(ISPRecommendedService SPRecommendedService) : IRequestHandler<UpdateSPRecommendedCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSPRecommendedCommand request, CancellationToken cancellationToken)
        {
            var SPRecommendedInDb = await SPRecommendedService.GetSPRecommendedAsync(request.SPRecommendedRequest.Id);


            SPRecommendedInDb.Id = request.SPRecommendedRequest.Id;
            SPRecommendedInDb.ConfigType = request.SPRecommendedRequest.ConfigType;
            SPRecommendedInDb.ConfigValue = request.SPRecommendedRequest.ConfigValue;
            SPRecommendedInDb.HscCode = request.SPRecommendedRequest.HscCode;
            SPRecommendedInDb.PartNo = request.SPRecommendedRequest.PartNo;
            SPRecommendedInDb.QtyRecommended = request.SPRecommendedRequest.QtyRecommended;
            SPRecommendedInDb.ServiceReportId = request.SPRecommendedRequest.ServiceReportId;
            SPRecommendedInDb.UpdatedBy = request.SPRecommendedRequest.UpdatedBy;


            var updateSPRecommendedId = await SPRecommendedService.UpdateSPRecommendedAsync(SPRecommendedInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSPRecommendedId, message: "Record updated successfully.");
        }
    }
}
