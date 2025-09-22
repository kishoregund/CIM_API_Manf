using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetSparepartQuery : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }

    public class GetSparepartQueryHandler(ISparepartService sparepartService)
        : IRequestHandler<GetSparepartQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSparepartQuery request, CancellationToken cancellationToken)
        {
            var SparepartInDb = (await sparepartService.GetSparepartAsync(request.Id)).Adapt<SparepartResponse>();

            if (SparepartInDb is not null)
            {
                return await ResponseWrapper<SparepartResponse>.SuccessAsync(data: SparepartInDb);
            }

            return await ResponseWrapper<SparepartResponse>.SuccessAsync(message: "Sparepart does not exists.");
        }
    }
}
