namespace Application.Features.Identity.Users.Queries
{
    public class GetAllUsersQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetAllUsersQueryHandler(IUserService userService) : IRequestHandler<GetAllUsersQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userService.GetUsersAsync(cancellationToken);
            return await ResponseWrapper<List<UserDto>>.SuccessAsync(data: users);
        }
    }
}
