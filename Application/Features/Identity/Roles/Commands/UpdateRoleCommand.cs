namespace Application.Features.Identity.Roles.Commands
{
    public class UpdateRoleCommand : IRequest<IResponseWrapper>
    {
        public UpdateRoleRequest UpdateRole { get; set; }
    }

    public class UpdateRoleCommandHandler(IRoleService roleService) : IRequestHandler<UpdateRoleCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var updatedRole = await roleService.UpdateAsync(request.UpdateRole);
            return await ResponseWrapper<string>.SuccessAsync(data: updatedRole, message: "Role updated successfully.");
        }
    }
}
