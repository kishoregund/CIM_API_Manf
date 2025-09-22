using Application.Features.Instruments.Commands;
using Application.Features.Instruments;
using Application.Features.Instruments.Requests;
using Domain.Entities;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.JavaScript;
using System.Diagnostics.Metrics;

namespace Application.Features.Instruments.Commands
{
    public class UpdateInstrumentCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public InstrumentRequest InstrumentRequest { get; set; }
    }

    public class UpdateInstrumentCommandHandler(IInstrumentService InstrumentService, IInstrumentSparesService instrumentSparesService) : IRequestHandler<UpdateInstrumentCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateInstrumentCommand request, CancellationToken cancellationToken)
        {
            var InstrumentInDb = await InstrumentService.GetInstrumentEntityAsync(request.InstrumentRequest.Id);


            InstrumentInDb.Id = request.InstrumentRequest.Id;
            InstrumentInDb.BrandId = request.InstrumentRequest.BrandId;
            InstrumentInDb.BusinessUnitId = request.InstrumentRequest.BusinessUnitId;
            InstrumentInDb.ManufId = request.InstrumentRequest.ManufId;
            InstrumentInDb.Image = request.InstrumentRequest.Image;
            InstrumentInDb.InsMfgDt = request.InstrumentRequest.InsMfgDt;
            InstrumentInDb.InsType = request.InstrumentRequest.InsType;
            InstrumentInDb.InsVersion = request.InstrumentRequest.InsVersion;
            InstrumentInDb.SerialNos = request.InstrumentRequest.SerialNos;
            InstrumentInDb.UpdatedBy = request.InstrumentRequest.UpdatedBy;

            var updateInstrumentId = await InstrumentService.UpdateInstrumentAsync(InstrumentInDb);
            request.InstrumentRequest.Spares.ForEach(x => x.InstrumentId = request.InstrumentRequest.Id);
            var insSpares = await instrumentSparesService.UpdateInsertInstrumentSparesAsync(request.InstrumentRequest.Spares.Adapt<List<InstrumentSpares>>().ToList());
            return await ResponseWrapper<Guid>.SuccessAsync(data: updateInstrumentId, message: "Record updated successfully.");
        }
    }
}
