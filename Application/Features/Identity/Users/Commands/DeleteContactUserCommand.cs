namespace Application.Features.Identity.Users.Commands
{
    public class DeleteContactUserCommand : IRequest<IResponseWrapper>
    {
        public ChangeUserStatusRequest ChangeUserStatus { get; set; }
    }

    public class DeleteContactUserCommandHandler(IUserService userService)
        : IRequestHandler<DeleteContactUserCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteContactUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.DeleteContactUserAsync(request.ChangeUserStatus.ContactType, request.ChangeUserStatus.Activation, Guid.Parse(request.ChangeUserStatus.ContactId));
            return await ResponseWrapper<bool>.SuccessAsync(data: user, message: "User deleted successfully");
        }
    }
}
