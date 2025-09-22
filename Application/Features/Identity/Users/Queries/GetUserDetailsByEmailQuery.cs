using Application.Features.Identity.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Queries
{
    public class GetUserDetailsByEmailQuery : IRequest<IResponseWrapper>
    {
        public string Email { get; set; }
    }

    public class GetUserDetailsByEmailQueryHandler(IUserService userService) : IRequestHandler<GetUserDetailsByEmailQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUserDetailsByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserDetailsAsync(request.Email, cancellationToken);
            return await ResponseWrapper<UserDetailsDto>.SuccessAsync(data: user);
        }
    }
}
