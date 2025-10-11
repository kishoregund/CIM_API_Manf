using Application.Features.Instruments;
using Application.Features.Instruments.Responses;

namespace Application.Features.InstrumentAllocations.Queries
{
    public class GetInstrumentAllocationByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid InstrumentAllocationId { get; set; }
    }

    public class GetInstrumentAllocationByIdQueryHandler(IInstrumentAllocationService InstrumentAllocationService) : IRequestHandler<GetInstrumentAllocationByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentAllocationByIdQuery request, CancellationToken cancellationToken)
        {
            var InstrumentAllocationInDb = (await InstrumentAllocationService.GetInstrumentAllocationEntityAsync(request.InstrumentAllocationId)).Adapt<InstrumentAllocationResponse>();

            if (InstrumentAllocationInDb is not null)
            {
                return await ResponseWrapper<InstrumentAllocationResponse>.SuccessAsync(data: InstrumentAllocationInDb);
            }
            return await ResponseWrapper<InstrumentAllocationResponse>.SuccessAsync(message: "InstrumentAllocation does not exists.");
        }
    }
}
