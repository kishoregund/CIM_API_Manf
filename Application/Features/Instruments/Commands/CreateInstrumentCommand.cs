using Application.Features.Instruments.Responses;
using Application.Features.Instruments.Commands;
using Application.Features.Instruments;
using Application.Features.Instruments.Requests;
using System.Transactions;

namespace Application.Features.Instruments.Commands
{
    public class CreateInstrumentCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public InstrumentRequest InstrumentRequest { get; set; }
    }

    public class CreateInstrumentCommandHandler(IInstrumentService InstrumentService, IInstrumentSparesService instrumentSparesService) : IRequestHandler<CreateInstrumentCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateInstrumentCommand request, CancellationToken cancellationToken)
        {
            var newInstrument = request.InstrumentRequest.Adapt<Instrument>();
            var InstrumentId = await InstrumentService.CreateInstrumentAsync(newInstrument);

            request.InstrumentRequest.Spares.ForEach(x => x.InstrumentId = InstrumentId);

            var newInstrumentSpares = request.InstrumentRequest.Spares.Adapt<List<InstrumentSpares>>();
            var spares =  instrumentSparesService.CreateInstrumentSparesAsync(newInstrumentSpares);


            return await ResponseWrapper<Guid>.SuccessAsync(data: InstrumentId, message: "Record saved successfully.");
        }
    }
}
