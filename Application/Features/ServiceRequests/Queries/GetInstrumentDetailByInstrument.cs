using Application.Features.Customers;
using Application.Features.Customers.Responses;
using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetInstrumentDetailByInstrumentQuery : IRequest<IResponseWrapper>
    {
        public Guid InstrumentId { get; set; }
        public Guid SiteId { get; set; }
    }

    public class GetInstrumentDetailByInstrumentQueryHandler(IServiceRequestService serviceRequestService) : IRequestHandler<GetInstrumentDetailByInstrumentQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentDetailByInstrumentQuery request, CancellationToken cancellationToken)
        {
            var custInstrumentInDb = await serviceRequestService.GetInstrumentDetailByInstrAsync(request.InstrumentId, request.SiteId);

            if (custInstrumentInDb is not null)
            {
                return await ResponseWrapper<SRInstrumentResponse>.SuccessAsync(data: custInstrumentInDb);
            }
            return await ResponseWrapper<SRInstrumentResponse>.SuccessAsync(message: "Service Request Instrument does not exists.");
        }
    }
}
