using Application.Features.ServiceReports.Requests;
using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Commands
{
    public class CreateSPRecommendedCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SPRecommendedRequest SPRecommendedRequest { get; set; }
    }

    public class CreateSPRecommendedCommandHandler(ISPRecommendedService SPRecommendedService) : IRequestHandler<CreateSPRecommendedCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSPRecommendedCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSPRecommended = request.SPRecommendedRequest.Adapt<SPRecommended>();

            var SPRecommendedId = await SPRecommendedService.CreateSPRecommendedAsync(newSPRecommended);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SPRecommendedId, message: "Record saved successfully.");
        }
    }
}
