namespace Application.Features.Identity.Users.Queries
{
    public class GetUserRegionsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetUserRegionsQueryHandler(IUserService userService) : IRequestHandler<GetUserRegionsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUserRegionsQuery request, CancellationToken cancellationToken)
        {
            var userRegions = await userService.GetUserRegionsAsync();
            return await ResponseWrapper<List<string>>.SuccessAsync(data: userRegions);
        }
    }
}
