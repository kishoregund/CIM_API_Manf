using Application.Features.Spares.Requests;

namespace Application.Features.Spares.Commands
{
    public class UpdateSparepartCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SparepartRequest SparepartRequest { get; set; }
    }

    public class UpdateSparepartCommandHandler(ISparepartService sparepartService)
        : IRequestHandler<UpdateSparepartCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSparepartCommand request, CancellationToken cancellationToken)
        {
            var sparepartInDb = await sparepartService.GetSparepartEntityAsync(request.SparepartRequest.Id);
            sparepartInDb.Id = request.SparepartRequest.Id;
            sparepartInDb.ConfigTypeId = request.SparepartRequest.ConfigTypeId;
            sparepartInDb.ConfigValueId = request.SparepartRequest.ConfigValueId;
            sparepartInDb.CurrencyId = request.SparepartRequest.CurrencyId;
            sparepartInDb.CountryId = request.SparepartRequest.CountryId;
            sparepartInDb.DescCatalogue = request.SparepartRequest.DescCatalogue;
            sparepartInDb.HsCode = request.SparepartRequest.HsCode;
            sparepartInDb.Image = request.SparepartRequest.Image;
            sparepartInDb.IsObselete = request.SparepartRequest.IsObselete;
            sparepartInDb.ItemDesc = request.SparepartRequest.ItemDesc;
            sparepartInDb.PartNo = request.SparepartRequest.PartNo;
            sparepartInDb.PartType = request.SparepartRequest.PartType;
            sparepartInDb.Price = request.SparepartRequest.Price;
            sparepartInDb.Qty = request.SparepartRequest.Qty;
            sparepartInDb.ReplacePartNoId = request.SparepartRequest.ReplacePartNoId;
            sparepartInDb.UpdatedBy = request.SparepartRequest.UpdatedBy;

            var updateSparepartId = await sparepartService.UpdateSparepartAsync(sparepartInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSparepartId,
                message: "Record updated successfully.");
        }
    }
}