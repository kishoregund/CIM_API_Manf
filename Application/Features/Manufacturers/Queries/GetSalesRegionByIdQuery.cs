using Application.Features.Manufacturers.Responses;

namespace Application.Features.Manufacturers.Queries
{
    public class GetSalesRegionByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid SalesRegionId { get; set; }
    }

    public class GetSalesRegionByIdQueryHandler(ISalesRegionService SalesRegionService, ISalesRegionContactService salesRegionContactService) : IRequestHandler<GetSalesRegionByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSalesRegionByIdQuery request, CancellationToken cancellationToken)
        {
            var salesRegionInDb = (await SalesRegionService.GetSalesRegionAsync(request.SalesRegionId)).Adapt<SalesRegionResponse>();
            salesRegionInDb.SalesRegionContacts = (await salesRegionContactService.GetSalesRegionContactsAsync(request.SalesRegionId)).Adapt<List<SalesRegionContactResponse>>();

            if (salesRegionInDb is not null)
            {
                return await ResponseWrapper<SalesRegionResponse>.SuccessAsync(data: salesRegionInDb);
            }
            return await ResponseWrapper<SalesRegionResponse>.SuccessAsync(message: "SalesRegion does not exists.");
        }
    }
}
