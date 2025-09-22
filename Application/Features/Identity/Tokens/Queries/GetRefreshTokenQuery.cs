namespace Application.Features.Identity.Tokens.Queries
{
    public class GetRefreshTokenQuery : IRequest<IResponseWrapper>
    {
        public RefreshTokenRequest RefreshToken { get; set; }
    }

    public class GetRefreshTokenQueryHandler(ITokenService tokenService) : IRequestHandler<GetRefreshTokenQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var refreshToken = await tokenService.RefreshTokenAsync(request.RefreshToken);
            return await ResponseWrapper<TokenResponse>.SuccessAsync(refreshToken);
        }
    }
}
