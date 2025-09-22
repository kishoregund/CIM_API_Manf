namespace Application.Features.Identity.Users.Commands
{
    public class UpdateUserStatusCommand : IRequest<IResponseWrapper>
    {
        public ChangeUserStatusRequest ChangeUserStatus { get; set; }
    }

    public class UpdateUserStatusCommandHandler(IUserService userService) 
        : IRequestHandler<UpdateUserStatusCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            var userId = await userService
                .ActivateOrDeactivateAsync(request.ChangeUserStatus.ContactType, request.ChangeUserStatus.Activation, Guid.Parse(request.ChangeUserStatus.ContactId));
            return await ResponseWrapper<string>
                .SuccessAsync(data: userId, message: request.ChangeUserStatus.Activation ? 
                    "User activated successfully" : "User de-activated successfully");
        }
    }
}
