using Application.Exceptions;
using Application.Features.Identity.Tokens;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Identity.Auth.Jwt;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity.Tokens
{
    public class TokenService(UserManager<ApplicationUser> _userManager, IMultiTenantContextAccessor<CIMTenantInfo> _tenantContextAccessor, IOptions<JwtSettings> jwtSettings) : ITokenService
    {
        private readonly UserManager<ApplicationUser> userManager = _userManager;
        private readonly IMultiTenantContextAccessor<CIMTenantInfo> tenantContextAccessor = _tenantContextAccessor;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<TokenResponse> LoginAsync(TokenRequest request)
        {
            bool isRoot = true;
            if (!tenantContextAccessor.MultiTenantContext.TenantInfo.IsActive)
            {
                throw new UnauthorizedException("Tenant Not Active. Please contact the admin.");
            }

            var userInDb = await userManager.FindByEmailAsync(request.Email)
                ?? throw new UnauthorizedException("Authentication not successful.");

            if (!await userManager.CheckPasswordAsync(userInDb, request.Password))
            {
                throw new UnauthorizedException("Incorrect Username or Password.");
            }

            if (!userInDb.IsActive)
            {
                throw new UnauthorizedException("User Not Active. Please contact the admin.");
            }

            if (tenantContextAccessor.MultiTenantContext.TenantInfo.Id != TenancyConstants.Root.Id)
            {
                isRoot = false;
                if (tenantContextAccessor.MultiTenantContext.TenantInfo.ValidUpTo < DateTime.UtcNow)
                {
                    throw new UnauthorizedException("Tenant subscription has expired. Please contact admin.");
                }
            }

            // Generate token
            TokenResponse tokenResponse = await GenerateTokensAndUpdateUserAsync(userInDb);
            tokenResponse.IsRoot = isRoot;

            return tokenResponse;
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var userPrincipal = GetClaimsPrincipalFromExpiredToken(request.CurrentJwtToken);
            var userEmail = userPrincipal.GetEmail();

            var userIndDb = await userManager.FindByEmailAsync(userEmail)
                ?? throw new UnauthorizedException("Authentication not successful.");

            return await GenerateTokensAndUpdateUserAsync(userIndDb);
        }

        private ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string expiredToken)
        {
            var tkValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = ClaimTypes.Role,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(expiredToken, tkValidationParams, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedException("Invaid token. Failed to create refresh token");
            }

            return principal;
        }

        private async Task<TokenResponse> GenerateTokensAndUpdateUserAsync(ApplicationUser user)
        {
            string newToken = GenerateJwt(user);

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryTimeInDays);

            await userManager.UpdateAsync(user);

            return new()
            {                
                JwtToken = newToken,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryDate = user.RefreshTokenExpiryTime
            };
        }

        private string GenerateJwt(ApplicationUser user)
        {
            return GenerateEncryptedToken(GetSigningCredentials(), GetUserClaims(user));
        }

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryTimeInMinutes),
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private IEnumerable<Claim> GetUserClaims(ApplicationUser user)
        {
            return
            [
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimConstants.Tenant, tenantContextAccessor.MultiTenantContext.TenantInfo.Id),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            ];
        }
    }
}
