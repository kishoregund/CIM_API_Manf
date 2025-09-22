using Application.Features.Spares.Requests;

namespace Application.Features.Spares.Commands
{
    public class UpdateOfferRequestProcessCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public OfferRequestProcessRequest OfferRequestProcessRequest { get; set; }
    }

    public class UpdateOfferRequestProcessCommandHandler(IOfferRequestProcessService OfferRequestProcessService)
        : IRequestHandler<UpdateOfferRequestProcessCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateOfferRequestProcessCommand request, CancellationToken cancellationToken)
        {
            var OfferRequestProcessInDb = await OfferRequestProcessService.GetOfferRequestProcessEntityAsync(request.OfferRequestProcessRequest.Id);
            OfferRequestProcessInDb.Id = request.OfferRequestProcessRequest.Id;
            OfferRequestProcessInDb.Stage = request.OfferRequestProcessRequest.Stage;
            OfferRequestProcessInDb.OfferRequestId= request.OfferRequestProcessRequest.OfferRequestId;
            OfferRequestProcessInDb.UserId = request.OfferRequestProcessRequest.UserId;
            OfferRequestProcessInDb.StageIndex = request.OfferRequestProcessRequest.StageIndex;
            OfferRequestProcessInDb.BaseCurrencyAmt = request.OfferRequestProcessRequest.BaseCurrencyAmt;
            OfferRequestProcessInDb.Comments = request.OfferRequestProcessRequest.Comments;
            OfferRequestProcessInDb.Index = request.OfferRequestProcessRequest.Index;
            OfferRequestProcessInDb.IsActive = request.OfferRequestProcessRequest.IsActive;
            OfferRequestProcessInDb.IsDeleted = request.OfferRequestProcessRequest.IsDeleted;
            OfferRequestProcessInDb.IsCompleted = request.OfferRequestProcessRequest.IsCompleted;
            OfferRequestProcessInDb.PayAmt = request.OfferRequestProcessRequest.PayAmt;
            OfferRequestProcessInDb.PayAmtCurrencyId = request.OfferRequestProcessRequest.PayAmtCurrencyId;
            OfferRequestProcessInDb.PaymentTypeId = request.OfferRequestProcessRequest.PaymentTypeId;

            var updateOfferRequestProcessId = await OfferRequestProcessService.UpdateOfferRequestProcessAsync(OfferRequestProcessInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateOfferRequestProcessId,
                message: "Record updated successfully.");
        }
    }
}