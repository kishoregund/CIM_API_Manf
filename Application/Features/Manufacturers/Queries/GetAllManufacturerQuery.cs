using Application.Features.Manufacturers.Responses;


namespace Application.Features.Manufacturers.Queries
{
    public class GetAllManufacturerQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetAllManufacturerQueryHandler(IManufacturerService Manufacturerervice) : IRequestHandler<GetAllManufacturerQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllManufacturerQuery request, CancellationToken cancellationToken)
        {
            var ManufacturerInDb = await Manufacturerervice.GetManufacturersAsync();

            if (ManufacturerInDb.Count > 0)
            {
                return await ResponseWrapper<List<ManufacturerResponse>>.SuccessAsync(data: ManufacturerInDb.Adapt<List<ManufacturerResponse>>());
            }
            return await ResponseWrapper<ManufacturerResponse>.SuccessAsync(message: "No Manufacturer were found.");
        }
    }
}
