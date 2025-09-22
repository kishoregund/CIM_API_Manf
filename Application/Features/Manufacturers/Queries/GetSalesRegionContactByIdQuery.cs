using Application.Features.Manufacturers.Responses;


namespace Application.Features.Manufacturers.Queries
{
    public class GetSalesRegionContactByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid SalesRegionContactId { get; set; }
    }

    public class GetSalesRegionContactByIdQueryHandler(ISalesRegionContactService SalesRegionContactService) : IRequestHandler<GetSalesRegionContactByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSalesRegionContactByIdQuery request, CancellationToken cancellationToken)
        {
            var salesRegionContactInDb = (await SalesRegionContactService.GetSalesRegionContactAsync(request.SalesRegionContactId)).Adapt<SalesRegionContactResponse>();

            if (salesRegionContactInDb is not null)
            {
                return await ResponseWrapper<SalesRegionContactResponse>.SuccessAsync(data: salesRegionContactInDb);
            }
            return await ResponseWrapper<SalesRegionContactResponse>.SuccessAsync(message: "SalesRegionContact does not exists.");
        }
    }
}
