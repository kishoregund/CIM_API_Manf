namespace Application.Features.Identity.Users.Commands
{
    public class ChangeUserPasswordCommand : IRequest<IResponseWrapper>
    {
        public ChangePasswordRequest ChangePassword { get; set; }
    }

    public class ChangeUserPasswordCommandHandler(IUserService userService) :
        IRequestHandler<ChangeUserPasswordCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var userId = await userService.ChangePasswordAsync(request.ChangePassword);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "User password changed successfully.");
        }
    }
}
