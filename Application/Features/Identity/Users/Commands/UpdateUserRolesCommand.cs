namespace Application.Features.Identity.Users.Commands
{
    public class UpdateUserRolesCommand : IRequest<IResponseWrapper>
    {
        public string RoleId { get; set; }
        public UserRolesRequest UserRolesRequest { get; set; }
    }

    public class UpdateUserRolesCommandHandler(IUserService userService) : IRequestHandler<UpdateUserRolesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var userId = await userService.AssignRolesAsync(request.RoleId, request.UserRolesRequest);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "User roles updated successfully.");
        }
    }
}
