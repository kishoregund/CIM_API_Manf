﻿namespace Application.Features.Identity.Users.Queries
{
    public class GetUserRolesQuery : IRequest<IResponseWrapper>
    {
        public string UserId { get; set; }
    }

    public class GetUserRolesQueryHandler(IUserService userService) : IRequestHandler<GetUserRolesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var userRoles = await userService.GetUserRolesAsync(request.UserId, cancellationToken);
            return await ResponseWrapper<List<UserRoleDto>>.SuccessAsync(data: userRoles);
        }
    }
}
