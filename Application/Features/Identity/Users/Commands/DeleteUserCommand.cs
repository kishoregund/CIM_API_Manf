namespace Application.Features.Identity.Users.Commands
{
    public class DeleteUserCommand : IRequest<IResponseWrapper>
    {
        public string UserId { get; set; }
    }

    public class DeleteUserCommandHandler(IUserService userService)
        : IRequestHandler<DeleteUserCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userId = await userService.DeleteUserAsync(request.UserId);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "User deleted successfully" );
        }
    }
}
