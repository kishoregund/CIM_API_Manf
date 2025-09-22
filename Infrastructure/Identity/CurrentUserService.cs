using Application.Exceptions;
using Application.Features.Identity.Users;
using Infrastructure.Persistence.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public class CurrentUserService(IConfiguration configuration) : ICurrentUserService
    {
        private ClaimsPrincipal _principal;

        public string Name => _principal.Identity.Name;

        public IEnumerable<Claim> GetUserClaims()
        {
            return _principal.Claims;
        }

        public string GetUserEmail()
        {
            if (IsAuthenticated())
            {
                return _principal.GetEmail();
            }
            return string.Empty;
        }

        public string GetUserId()
        {
            if (IsAuthenticated())
            {
                return _principal.GetUserId();
            }
            return string.Empty;
        }

        public string GetUserTenant()
            => IsAuthenticated() ? _principal.GetTenant() : string.Empty;
        //{
        //if (IsAuthenticated())
        //{
        //    return _principal.GetTenant();
        //}
        //return string.Empty;
        //}

        public bool IsAuthenticated()
            => _principal.Identity.IsAuthenticated;


        public bool IsInRole(string roleName)
        {
            return _principal.IsInRole(roleName);
        }

        public void SetCurrentUser(ClaimsPrincipal principal)
        {
            if (_principal is not null)
            {
                throw new ConflictException("Invalid operation on claim.");
            }

            _principal = principal;
        }

        public string GetLoggedinUserTenant(string emailId)
        {
            SqlConnection con = new(configuration.GetConnectionString("DefaultConnection"));
            con.Open();
            DataTable dt = new();
            SqlDataAdapter da = new("select * from Multitenancy.TenantUsers where email = '" + emailId + "'", con);
            da.Fill(dt);
            con.Close();

            return dt.Rows.Count > 0 ? dt.Rows[0]["TenantId"].ToString() : string.Empty;

        }
    }
}
