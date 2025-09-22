﻿namespace Application.Features.Identity.Roles.Commands
{
    public class UpdateRolePermissionsCommand : IRequest<IResponseWrapper>
    {
        public UpdateRolePermissionsRequest UpdateRolePermissions { get; set; }
    }

    public class UpdateRolePermissionsCommandHandler(IRoleService roleService) 
        : IRequestHandler<UpdateRolePermissionsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var message = await roleService.UpdatePermissionsAsync(request.UpdateRolePermissions);
            return await ResponseWrapper.SuccessAsync(message: message);
        }
    }
}
