namespace Application.Features.Identity.Roles.Commands
{
    public class CreateRoleCommand : IRequest<IResponseWrapper>
    {
        public CreateRolePermissionRequest RoleRequest { get; set; }
    }

    public class CreateRoleCommandHandler(IRoleService roleService) : IRequestHandler<CreateRoleCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var newRole = await roleService.CreateAsync(request.RoleRequest);
            return await ResponseWrapper<string>.SuccessAsync(data: newRole, message: "Role created successfully.");
        }
    }
}
