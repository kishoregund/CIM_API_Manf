using Application.Features.Instruments.Responses;
using Application.Features.Instruments.Commands;
using Application.Features.Instruments;
using Application.Features.Instruments.Requests;
using System.Transactions;

namespace Application.Features.Instruments.Commands
{
    public class CreateInstrumentAllocationCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public InstrumentAllocationRequest InstrumentAllocationRequest { get; set; }
    }

    public class CreateInstrumentAllocationCommandHandler(IInstrumentAllocationService instrumentAllocationService) : IRequestHandler<CreateInstrumentAllocationCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateInstrumentAllocationCommand request, CancellationToken cancellationToken)
        {
            var newInstrumentAllocation = request.InstrumentAllocationRequest.Adapt<InstrumentAllocation>();
            var InstrumentAllocationId = await instrumentAllocationService.CreateInstrumentAllocationAsync(newInstrumentAllocation);

            return await ResponseWrapper<Guid>.SuccessAsync(data: InstrumentAllocationId, message: "Record saved successfully.");
        }
    }
}
