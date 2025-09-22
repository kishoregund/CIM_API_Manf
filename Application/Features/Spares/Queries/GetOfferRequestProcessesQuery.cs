using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetOfferRequestProcessesQuery : IRequest<IResponseWrapper>
    {
        public Guid OfferRequestId { get; set; }
    }

    public class GetOfferRequestProcessesQueryHandler(IOfferRequestProcessService OfferRequestProcesseservice) : IRequestHandler<GetOfferRequestProcessesQuery, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(GetOfferRequestProcessesQuery request, CancellationToken cancellationToken)
        {
            var OfferRequestInDb = (await OfferRequestProcesseservice.GetOfferRequestProcessesAsync(request.OfferRequestId));

            if (OfferRequestInDb.Count > 0)
            {
                return await ResponseWrapper<List<OfferRequestProcessResponse>>.SuccessAsync(data: OfferRequestInDb);
            }

            return await ResponseWrapper<OfferRequestProcessResponse>.SuccessAsync(message: "No OfferRequestProcesses were found.");
        }
    }
}