using Application.Features.AppBasic.Responses;
using Application.Features.Tenancy.Models;

namespace Application.Features.AppBasic.Queries
{
    public class GetModalDataQuery : IRequest<IResponseWrapper>
    {
        
    }

    public class GetModalDataQueryHandler(IAppBasicService appBasicService) : IRequestHandler<GetModalDataQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetModalDataQuery request, CancellationToken cancellationToken)
        {
            var modalDataInDb = await appBasicService.GetModalDataAsync();

            if (modalDataInDb.BusinessUnits.Count > 0)
            {
                return await ResponseWrapper<ModalDataResponse>.SuccessAsync(data: modalDataInDb.Adapt<ModalDataResponse>());
            }
            return await ResponseWrapper<ModalDataResponse>.SuccessAsync(message: "No Modal Data was found.");
        }
    }
}