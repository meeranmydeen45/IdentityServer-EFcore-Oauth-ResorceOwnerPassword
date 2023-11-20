using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace OauthServer.Services
{
    public static class UserService
    {
        public static void CreateUser(this IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var user = new IdentityUser("Dav");
                userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
                userManager.AddClaimAsync(user, new Claim("user.role", "admin")).GetAwaiter().GetResult();
            }
        }
    }
}
