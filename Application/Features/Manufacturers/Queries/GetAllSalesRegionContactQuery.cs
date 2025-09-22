using Application.Features.Manufacturers;
using Application.Features.Manufacturers.Responses;

namespace Application.Features.Manufacturers.Queries
{
    public class GetAllSalesRegionContactQuery : IRequest<IResponseWrapper>
    {
        public Guid SalesRegionId { get; set; }
    }

    public class GetAllSalesRegionContactQueryHandler(ISalesRegionContactService SalesRegionContactervice) : IRequestHandler<GetAllSalesRegionContactQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllSalesRegionContactQuery request, CancellationToken cancellationToken)
        {
            var SalesRegionContactInDb = await SalesRegionContactervice.GetSalesRegionContactsAsync(request.SalesRegionId);

            if (SalesRegionContactInDb.Count > 0)
            {
                return await ResponseWrapper<List<SalesRegionContactResponse>>.SuccessAsync(data: SalesRegionContactInDb.Adapt<List<SalesRegionContactResponse>>());
            }
            return await ResponseWrapper<List<SalesRegionContactResponse>>.SuccessAsync(message: "No SalesRegionContact were found.");
        }
    }
}
