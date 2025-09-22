using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetSparepartByPartNoQuery : IRequest<IResponseWrapper>
    {
        public string partNo { get; set; }
    }

    public class GetSparepartByPartNoQueryHandler(ISparepartService sparepartService)
        : IRequestHandler<GetSparepartByPartNoQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSparepartByPartNoQuery request, CancellationToken cancellationToken)
        {
            var SparepartInDb = (await sparepartService.GetSparepartByPartNoAsync(request.partNo)).Adapt<SparepartResponse>();

            if (SparepartInDb is not null)
            {
                return await ResponseWrapper<SparepartResponse>.SuccessAsync(data: SparepartInDb);
            }

            return await ResponseWrapper<SparepartResponse>.SuccessAsync(message: "Sparepart does not exists.");
        }
    }
}
