using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace OauthServer.Services
{
    public class ClientStore : IClientStore
    {
        private readonly AuthDbContext authDbContext;

        public ClientStore(AuthDbContext _authDbContext, IHttpContextAccessor contextAccessor)
        {
            authDbContext = _authDbContext;

        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await authDbContext.Client.Where(x => x.ClientId == clientId).FirstOrDefaultAsync();

            if (client == null)
            {
                return new Client();
            }
            
            var clientGrantTypes = await authDbContext.ClientGrantTypes
                       .Where(x => x.ClientId == client.Id).Select(x => x.GrantType)
                       .ToListAsync();
            var clientSecret = await authDbContext.ClientSecrets
                       .Where(x => x.ClientId == client.Id)
                       .ToListAsync();
            var clientScopes = await authDbContext.ClientScopes
                       .Where(x => x.ClientId == client.Id).Select(x => x.Scope)
                       .ToListAsync();

            return new Client
            {
                ClientId = client.ClientId,
                //ClientSecrets = clientSecret.Select(x=> new Secret(x.Secrets.Sha256())).ToList(),
                AllowedGrantTypes = GrantTypes.Implicit,
                //RedirectUris = { "https://localhost:5001/signin-oidc" },
                RedirectUris = { "http://localhost:3000/about/" },
                AllowedScopes = clientScopes,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedCorsOrigins = { "http://localhost:3000" },
                AllowAccessTokensViaBrowser = true,
                
                //PostLogoutRedirectUris = { "https://localhost:4000/signout-callback-oidc" }
            };
        }
    }
}
