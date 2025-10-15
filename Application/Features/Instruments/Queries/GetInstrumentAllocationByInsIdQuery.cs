using Application.Features.Instruments;
using Application.Features.Instruments.Responses;

namespace Application.Features.InstrumentAllocations.Queries
{
    public class GetInstrumentAllocationByInsIdQuery : IRequest<IResponseWrapper>
    {
        public Guid InstrumentId { get; set; }
    }

    public class GetInstrumentAllocationByInsIdQueryHandler(IInstrumentAllocationService InstrumentAllocationService) : IRequestHandler<GetInstrumentAllocationByInsIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentAllocationByInsIdQuery request, CancellationToken cancellationToken)
        {
            var InstrumentAllocationInDb = (await InstrumentAllocationService.GetInstrumentAllocationByInsIdAsync(request.InstrumentId)).Adapt<InstrumentAllocationResponse>();

            if (InstrumentAllocationInDb is not null)
            {
                return await ResponseWrapper<InstrumentAllocationResponse>.SuccessAsync(data: InstrumentAllocationInDb);
            }
            return await ResponseWrapper<InstrumentAllocationResponse>.SuccessAsync(message: "InstrumentAllocation does not exists.");
        }
    }
}