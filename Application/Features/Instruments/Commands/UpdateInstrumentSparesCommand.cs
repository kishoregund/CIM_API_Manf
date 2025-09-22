
using Application.Features.Instruments.Requests;
using Domain.Entities;

namespace Application.Features.Instruments.Commands
{
    public class UpdateInstrumentSparesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public InstrumentSparesRequest InstrumentSparesRequest { get; set; }
    }

    public class UpdateInstrumentSparesCommandHandler(IInstrumentSparesService InstrumentSparesService) : IRequestHandler<UpdateInstrumentSparesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateInstrumentSparesCommand request, CancellationToken cancellationToken)
        {
            var InstrumentSparesInDb = await InstrumentSparesService.GetInstrumentSparesAsync(request.InstrumentSparesRequest.Id);

            InstrumentSparesInDb.Id = request.InstrumentSparesRequest.Id;
            InstrumentSparesInDb.InsQty = request.InstrumentSparesRequest.InsQty;
            InstrumentSparesInDb.InstrumentId = request.InstrumentSparesRequest.InstrumentId;
            InstrumentSparesInDb.ConfigTypeId = request.InstrumentSparesRequest.ConfigTypeId;
            InstrumentSparesInDb.ConfigValueId = request.InstrumentSparesRequest.ConfigValueId;
            InstrumentSparesInDb.SparepartId = request.InstrumentSparesRequest.SparepartId;
            InstrumentSparesInDb.UpdatedBy = request.InstrumentSparesRequest.UpdatedBy;

            var updateInstrumentSparesId = await InstrumentSparesService.UpdateInstrumentSparesAsync(InstrumentSparesInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateInstrumentSparesId, message: "Record updated successfully.");
        }
    }
}
