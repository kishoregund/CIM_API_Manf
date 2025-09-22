namespace Application.Features.Identity.Users.Queries
{
    public class GetUserByContactIdQuery : IRequest<IResponseWrapper>
    {
        public CreateUserRequest userRequest { get; set; }
    }

    public class GetUserByContactIdQueryHandler(IUserService userService) : IRequestHandler<GetUserByContactIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUserByContactIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByContactIdAsync(request.userRequest.ContactId, request.userRequest.ContactType, cancellationToken);
            return await ResponseWrapper<UserDto>.SuccessAsync(data: user);
        }
    }
}
