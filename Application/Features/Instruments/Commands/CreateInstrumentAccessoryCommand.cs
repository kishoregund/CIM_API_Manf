
using Application.Features.Instruments.Requests;

namespace Application.Features.Instruments.Commands
{
    public class CreateInstrumentAccessoryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public InstrumentAccessoryRequest InstrumentAccessoryRequest { get; set; }
    }

    public class CreateInsAccessoryCommandHandler(IInstrumentAccessoryService InsAccessoryService) : IRequestHandler<CreateInstrumentAccessoryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateInstrumentAccessoryCommand request, CancellationToken cancellationToken)
        {
            // map

            var newInsAccessory = request.InstrumentAccessoryRequest.Adapt<InstrumentAccessory>();

            var InsAccessoryId = await InsAccessoryService.CreateInstrumentAccessoryAsync(newInsAccessory);

            return await ResponseWrapper<Guid>.SuccessAsync(data: InsAccessoryId, message: "Record saved successfully.");
        }
    }
}
