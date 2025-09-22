namespace Application.Features.Identity.Roles.Queries
{
    public class GetRolesQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetRolesQueryHandler(IRoleService roleService) : IRequestHandler<GetRolesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = (await roleService.GetRolesAsync(cancellationToken)).Where(x=>x.Name != "Admin").ToList();
            return await ResponseWrapper<List<RoleDto>>.SuccessAsync(data: roles);
        }
    }
}
