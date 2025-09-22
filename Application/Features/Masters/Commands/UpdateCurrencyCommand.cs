using Application.Features.Masters.Requests;
using System.Reflection.Emit;

namespace Application.Features.Masters.Commands
{
    public class UpdateCurrencyCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CurrencyRequest CurrencyRequest { get; set; }
    }

    public class UpdateCurrencyCommandHandler(ICurrencyService CurrencyService) : IRequestHandler<UpdateCurrencyCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var CurrencyInDb = await CurrencyService.GetCurrencyAsync(request.CurrencyRequest.Id);

            CurrencyInDb.Id = request.CurrencyRequest.Id;
            CurrencyInDb.UpdatedBy = request.CurrencyRequest.UpdatedBy;
            CurrencyInDb.Code = request.CurrencyRequest.Code;
            CurrencyInDb.MCId = request.CurrencyRequest.MCId;
            CurrencyInDb.Minor_Unit = request.CurrencyRequest.Minor_Unit;
            CurrencyInDb.N_Code = request.CurrencyRequest.N_Code;
            CurrencyInDb.Symbol = request.CurrencyRequest.Symbol;
            CurrencyInDb.UpdatedBy = request.CurrencyRequest.UpdatedBy;


            var updateCurrencyId = await CurrencyService.UpdateCurrencyAsync(CurrencyInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateCurrencyId, message: "Record updated successfully.");
        }
    }
}