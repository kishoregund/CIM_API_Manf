using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetConfigSparepartQuery : IRequest<IResponseWrapper>
    {
        public Guid configTypeId { get; set; }
        //public Guid configValueId { get; set; }
    }

    public class GetConfigSparepartQueryHandler(ISparepartService sparepartService)
        : IRequestHandler<GetConfigSparepartQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetConfigSparepartQuery request, CancellationToken cancellationToken)
        {
            var SparepartInDb = (await sparepartService.GetConfigSparepartAsync(request.configTypeId)).Adapt<List<SparepartResponse>>();

            if (SparepartInDb is not null)
            {
                return await ResponseWrapper<List<SparepartResponse>>.SuccessAsync(data: SparepartInDb);
            }

            return await ResponseWrapper<SparepartResponse>.SuccessAsync(message: "Sparepart does not exists.");
        }
    }
}
