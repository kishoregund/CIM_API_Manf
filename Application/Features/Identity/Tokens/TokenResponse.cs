using Application.Features.Identity.Users.Models;
using Domain.Views;

namespace Application.Features.Identity.Tokens
{
    public class TokenResponse
    {
        public bool IsRoot { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
    }

    public class UserLoginResponse
    {
        public TokenResponse Token { get; set; }
        public VW_UserProfile UserDetails { get; set; }

    }
}
