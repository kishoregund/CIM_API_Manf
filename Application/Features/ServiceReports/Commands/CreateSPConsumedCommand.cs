
using Application.Features.ServiceReports.Responses;
using Application.Features.ServiceReports.Requests;

namespace Application.Features.ServiceReports.Commands
{
    public class CreateSPConsumedCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SPConsumedRequest SPConsumedRequest { get; set; }
    }

    public class CreateSPConsumedCommandHandler(ISPConsumedService SPConsumedService) : IRequestHandler<CreateSPConsumedCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSPConsumedCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSPConsumed = request.SPConsumedRequest.Adapt<SPConsumed>();

            var SPConsumedId = await SPConsumedService.CreateSPConsumedAsync(newSPConsumed);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SPConsumedId, message: "Record saved successfully.");
        }
    }
}
