using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace OauthServer.Services
{
    public class UserResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly AuthDbContext authDbContext;
        private readonly HttpContext? httpContext;

        public UserResourceOwnerPasswordValidator(AuthDbContext _authDbContext, IHttpContextAccessor accessor)
        {
            authDbContext = _authDbContext;
            httpContext = accessor.HttpContext;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await authDbContext.User.FirstOrDefaultAsync(x => x.UserName == context.UserName && x.Password == context.Password);
            if (user is not null)
            {
                var claims = new List<Claim>()
                {
                    new Claim("User.Role", user.Role)
                };

                context.Result = new GrantValidationResult(subject:context.UserName, authenticationMethod: "custom-claim", claims: claims);
                return;
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }
    }
}
