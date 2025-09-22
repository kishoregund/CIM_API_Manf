using Application.Exceptions;
using Application.Features.AMCS.Requests;

namespace Application.Features.AMCS.Commands
{
    public class UpdateAmcStagesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public AmcStagesRequest Request { get; set; }
    }

    public class UpdateAmcStagesCommandHandler(IAmcStagesService amcStagesService)
        : IRequestHandler<UpdateAmcStagesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateAmcStagesCommand request, CancellationToken cancellationToken)
        {
            var amcStages = await amcStagesService.GetByIdAsync(request.Request.Id);

            amcStages.UserId = request.Request.UserId;
            amcStages.Comments = request.Request.Comments;
            amcStages.AMCId = request.Request.AMCId;
            amcStages.StageIndex = request.Request.StageIndex;
            amcStages.Stage = request.Request.Stage;
            amcStages.Index = request.Request.Index;
            amcStages.IsCompleted = request.Request.IsCompleted;
            amcStages.PaymentTypeId = request.Request.PaymentTypeId;
            amcStages.PayAmtCurrencyId = request.Request.PayAmtCurrencyId;
            amcStages.PayAmt = request.Request.PayAmt;

            var response = await amcStagesService.UpdateAsync(amcStages);
            return await ResponseWrapper<Guid>.SuccessAsync(data: response, message: "Record updated successfully.");
        }
    }
}