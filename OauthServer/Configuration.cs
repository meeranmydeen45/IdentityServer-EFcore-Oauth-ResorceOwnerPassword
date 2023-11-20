using IdentityServer4.Models;
using IdentityServer4;
using System.Security.Claims;
using IdentityModel;

namespace OauthServer
{
    public static class Configuration
    {
       public static IEnumerable<ApiScope> GetApiScopes =>
       new List<ApiScope>
       {
           new ApiScope("ApiOne"),
       };

        public static IEnumerable<Client> GetClients =>
        new List<Client>
        {
            new Client
            {
               ClientId = "testapp_one",
               ClientSecrets = { new Secret("client_secret".ToSha256())},
               AllowedGrantTypes = GrantTypes.ClientCredentials,
               AllowedScopes = { "ApiOne" }
            }
        }; 
    }
}
