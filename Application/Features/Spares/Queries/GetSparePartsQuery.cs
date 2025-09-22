using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetSparepartsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetSparepartsQueryHandler(ISparepartService sparepartService)
        : IRequestHandler<GetSparepartsQuery, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(GetSparepartsQuery request, CancellationToken cancellationToken)
        {
            var sparepartInDb = (await sparepartService.GetSparepartsAsync());

            if (sparepartInDb.Count > 0)
            {
                return await ResponseWrapper<List<SparepartResponse>>.SuccessAsync(
                    data: sparepartInDb.Adapt<List<SparepartResponse>>());
            }

            return await ResponseWrapper<List<SparepartResponse>>.SuccessAsync(message: "No Spareparts were found.");
        }
    }
}