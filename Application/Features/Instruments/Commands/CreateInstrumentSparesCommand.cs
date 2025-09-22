using Application.Features.Instruments.Requests;


namespace Application.Features.Instruments.Commands
{
    public class CreateInstrumentSparesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public InstrumentSparesRequest InstrumentSparesRequest { get; set; }
    }

#pragma warning disable CS9113 // Parameter is unread.
    public class CreateInstrumentSparesCommandHandler(IInstrumentSparesService InstrumentSparesService) : IRequestHandler<CreateInstrumentSparesCommand, IResponseWrapper>
#pragma warning restore CS9113 // Parameter is unread.
    {
        public async Task<IResponseWrapper> Handle(CreateInstrumentSparesCommand request, CancellationToken cancellationToken)
        {
            // map

            //var newInstrumentSpares = request.InstrumentSparesRequest.Adapt<InstrumentSpares>();

            //var InstrumentSparesId = await InstrumentSparesService.CreateInstrumentSparesAsync(newInstrumentSpares);

            return await ResponseWrapper<Guid>.SuccessAsync(data: Guid.Empty, message: "Record saved successfully.");
            //return await ResponseWrapper<Guid>.SuccessAsync(data: InstrumentSparesId, message: "InstrumentSpares created successfully.");
        }
    }
}
