using Application.Features.Identity.Users;
using Application.Features.Identity.Users.Queries;

namespace Application.Features.Identity.Tokens.Queries
{
    public class GetTokenQuery : IRequest<IResponseWrapper>
    {
        public TokenRequest TokenRequest { get; set; }
    }

    public class GetTokenQueryHandler(ITokenService tokenService, IUserService userService) : IRequestHandler<GetTokenQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            var token = await tokenService.LoginAsync(request.TokenRequest);
            var userDetails = await userService.GetLoggedInUserDetailsAsync(request.TokenRequest.Email, cancellationToken);
            if (!string.IsNullOrEmpty(request.TokenRequest.BusinessUnitId))
            {
                userDetails.SelectedBusinessUnitId = request.TokenRequest.BusinessUnitId;
                userDetails.SelectedBrandId = request.TokenRequest.BrandId;
            }
            var loginResponse = new UserLoginResponse { Token = token, UserDetails = userDetails };
            return await ResponseWrapper<UserLoginResponse>.SuccessAsync(data: loginResponse);

        }
    }
}
