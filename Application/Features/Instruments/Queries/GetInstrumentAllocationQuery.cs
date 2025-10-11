
using Application.Features.Instruments.Responses;
using Application.Models;

namespace Application.Features.Instruments.Queries
{
    public class GetInstrumentAllocationQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetInstrumentAllocationQueryHandler(IInstrumentAllocationService InstrumentAllocationervice) : IRequestHandler<GetInstrumentAllocationQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentAllocationQuery request, CancellationToken cancellationToken)
        {
            var InstrumentAllocationInDb = await InstrumentAllocationervice.GetInstrumentAllocationsAsync();

            if (InstrumentAllocationInDb.Count > 0)
            {
                return await ResponseWrapper<List<InstrumentAllocationResponse>>.SuccessAsync(data: InstrumentAllocationInDb.Adapt<List<InstrumentAllocationResponse>>());
            }
            return await ResponseWrapper<List<InstrumentAllocationResponse>>.SuccessAsync(message: "No InstrumentAllocation were found.");
        }
    }
}
