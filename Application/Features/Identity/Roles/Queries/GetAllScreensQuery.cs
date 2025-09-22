namespace Application.Features.Identity.Roles.Queries
{
    public class GetAllScreensQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetAllScreensQueryHandler(IRoleService roleService) : IRequestHandler<GetAllScreensQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllScreensQuery request, CancellationToken cancellationToken)
        {
            var screens = await roleService.GetAllScreensAsync();
            return await ResponseWrapper<List<ScreenPermissions>>.SuccessAsync(data: screens);
        }
    }
}
