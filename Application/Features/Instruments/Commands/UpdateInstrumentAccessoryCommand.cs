
using Application.Features.Instruments.Requests;

namespace Application.Features.Instruments.Commands
{
    public class UpdateInstrumentAccessoryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public InstrumentAccessoryRequest InstrumentAccessoryRequest { get; set; }
    }

    public class UpdateInstrumentAccessoryCommandHandler(IInstrumentAccessoryService InstrumentAccessoryService) : IRequestHandler<UpdateInstrumentAccessoryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateInstrumentAccessoryCommand request, CancellationToken cancellationToken)
        {
            var InstrumentAccessoryInDb = await InstrumentAccessoryService.GetInstrumentAccessoryAsync(request.InstrumentAccessoryRequest.Id);

            InstrumentAccessoryInDb.Id = request.InstrumentAccessoryRequest.Id;
            InstrumentAccessoryInDb.AccessoryDescription = request.InstrumentAccessoryRequest.AccessoryDescription;
            InstrumentAccessoryInDb.AccessoryName = request.InstrumentAccessoryRequest.AccessoryName;
            InstrumentAccessoryInDb.BrandName = request.InstrumentAccessoryRequest.BrandName;
            InstrumentAccessoryInDb.InstrumentId = request.InstrumentAccessoryRequest.InstrumentId;
            InstrumentAccessoryInDb.ModelName = request.InstrumentAccessoryRequest.ModelName;
            InstrumentAccessoryInDb.ModelNumber = request.InstrumentAccessoryRequest.ModelNumber;
            InstrumentAccessoryInDb.Quantity = request.InstrumentAccessoryRequest.Quantity;
            InstrumentAccessoryInDb.SerialNumber = request.InstrumentAccessoryRequest.SerialNumber;
            InstrumentAccessoryInDb.YearOfPurchase = request.InstrumentAccessoryRequest.YearOfPurchase;
            InstrumentAccessoryInDb.UpdatedBy = request.InstrumentAccessoryRequest.UpdatedBy;

            var updateInstrumentAccessoryId = await InstrumentAccessoryService.UpdateInstrumentAccessoryAsync(InstrumentAccessoryInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateInstrumentAccessoryId, message: "Record updated successfully.");
        }
    }
}
