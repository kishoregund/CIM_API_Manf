
using Application.Features.Instruments.Responses;
using Application.Models;

namespace Application.Features.Instruments.Queries
{
    public class GetInstrumentQuery : IRequest<IResponseWrapper>
    {
        public BUBrand BUBrandModel { get; set; }
    }

    public class GetInstrumentQueryHandler(IInstrumentService Instrumentervice) : IRequestHandler<GetInstrumentQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentQuery request, CancellationToken cancellationToken)
        {
            var InstrumentInDb = await Instrumentervice.GetInstrumentsAsync(request.BUBrandModel.BusinessUnitId, request.BUBrandModel.BrandId);

            if (InstrumentInDb.Count > 0)
            {
                return await ResponseWrapper<List<InstrumentResponse>>.SuccessAsync(data: InstrumentInDb.Adapt<List<InstrumentResponse>>());
            }
            return await ResponseWrapper<List<InstrumentResponse>>.SuccessAsync(message: "No Instrument were found.");
        }
    }
}
