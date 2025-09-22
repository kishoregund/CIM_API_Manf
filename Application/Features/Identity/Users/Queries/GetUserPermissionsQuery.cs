namespace Application.Features.Identity.Users.Queries
{
    public class GetUserPermissionsQuery : IRequest<IResponseWrapper>
    {
        public string UserId { get; set; }
    }

    public class GetUserPermissionsQueryHanlder(IUserService userService) 
        : IRequestHandler<GetUserPermissionsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await userService.GetPermissionsAsync(request.UserId, cancellationToken);
            return await ResponseWrapper<List<string>>.SuccessAsync(data: permissions);
        }
    }
}
