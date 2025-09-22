using Application.Features.AMCS.Requests;
using Domain.Entities;

namespace Application.Features.AMCS.Commands
{
    public class CreateAmcInstrumentCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public List<AmcInstrumentRequest> AMCInstruments { get; set; }
    }

    public class CreateAmcInstrumentCommandHandler(IAmcInstrumentService service)
        : IRequestHandler<CreateAmcInstrumentCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateAmcInstrumentCommand request,
            CancellationToken cancellationToken)
        {
            var entity = request.AMCInstruments.Adapt<List<AMCInstrument>>();
            var newEntity = await service.CreateAmcInstrument(entity);
            return await ResponseWrapper<bool>.SuccessAsync(data: newEntity, message: "AMC Instrument Added successfully.");
        }
    }
}