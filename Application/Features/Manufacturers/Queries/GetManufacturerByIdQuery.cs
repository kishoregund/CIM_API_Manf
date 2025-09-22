using Application.Features.Manufacturers.Responses;
using Application.Features.Manufacturers.Queries;
using Application.Features.Manufacturers;
using Application.Features.Distributors.Responses;
using Application.Features.Distributors;

namespace Application.Features.Manufacturers.Queries
{
    public class GetManufacturerByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ManufacturerId { get; set; }
    }

    public class GetManufacturerByIdQueryHandler(IManufacturerService ManufacturerService, ISalesRegionService salesRegionService) : IRequestHandler<GetManufacturerByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetManufacturerByIdQuery request, CancellationToken cancellationToken)
        {
            var manufacturerInDb = (await ManufacturerService.GetManufacturerAsync(request.ManufacturerId)).Adapt<ManufacturerResponse>();
            manufacturerInDb.SalesRegions = (await salesRegionService.GetSalesRegionsAsync(request.ManufacturerId)).Adapt<List<SalesRegionResponse>>();

            if (manufacturerInDb is not null)
            {
                return await ResponseWrapper<ManufacturerResponse>.SuccessAsync(data: manufacturerInDb);
            }
            return await ResponseWrapper<ManufacturerResponse>.SuccessAsync(message: "Manufacturer does not exists.");
        }
    }
}
