using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetListTypeByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ListTypeId { get; set; }
    }

    public class GetListTypeByIdQueryHandler(IListTypeItemsService ListTypeService) : IRequestHandler<GetListTypeByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetListTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var ListTypeInDb = (await ListTypeService.GetListTypeByIdAsync(request.ListTypeId)).Adapt<ListType>();

            if (ListTypeInDb is not null)
            {
                return await ResponseWrapper<ListType>.SuccessAsync(data: ListTypeInDb);
            }
            return await ResponseWrapper<ListType>.SuccessAsync(message: "ListType does not exists.");
        }
    }
}
