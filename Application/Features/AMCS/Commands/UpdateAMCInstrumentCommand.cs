using Application.Exceptions;
using Application.Features.AMCS.Requests;

namespace Application.Features.AMCS.Commands
{
    public class UpdateAmcInstrumentCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public AmcInstrumentRequest Request { get; set; }
    }
    public class UpdateAmcInstrumentHandler(IAmcInstrumentService instrumentService) : IRequestHandler<UpdateAmcInstrumentCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateAmcInstrumentCommand request, CancellationToken cancellationToken)
        {
            var amcInstrument = await instrumentService.GetByIdAsync(request.Request.Id);

            amcInstrument.SerialNos = request.Request.SerialNos;
            amcInstrument.InsVersion = request.Request.InsVersion;
            amcInstrument.Qty = request.Request.Qty;
            amcInstrument.Rate = request.Request.Rate;
            amcInstrument.Amount = request.Request.Amount;
            amcInstrument.InsTypeId = request.Request.InsTypeId;
            amcInstrument.AMCId = request.Request.AMCId;
            amcInstrument.InstrumentId = request.Request.InstrumentId;

            var response = await instrumentService.UpdateInstrumentAsync(amcInstrument);
            return await ResponseWrapper<Guid>.SuccessAsync(response, "AMC Instrument Updated Successfully.");
        }
    }
}
