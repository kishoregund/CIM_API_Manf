namespace Application.Features.Identity.Users.Commands
{
    public class CreateUserCommand : IRequest<IResponseWrapper>
    {
        public CreateUserRequest CreateUserRequest { get; set; }
    }

    public class CreateUserCommandHandler(IUserService userService) : IRequestHandler<CreateUserCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = await userService.CreateUserAsync(request.CreateUserRequest);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "User created successfully.");
        }
    }
}
