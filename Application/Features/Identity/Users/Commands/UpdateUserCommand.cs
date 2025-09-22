namespace Application.Features.Identity.Users.Commands
{
    public class UpdateUserCommand : IRequest<IResponseWrapper>
    {
        public UpdateUserRequest UpdateUserRequest { get; set; }
    }

    public class UpdateUserCommandHanlder(IUserService userService) : IRequestHandler<UpdateUserCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = await userService.UpdateUserAsync(request.UpdateUserRequest);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "User updated successfully.");
        }
    }
}
