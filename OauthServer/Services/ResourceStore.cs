
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace OauthServer.Services
{
    public class ResourceStore : IResourceStore
    {

        private readonly AuthDbContext authDbContext;
        public ResourceStore(AuthDbContext _authDbContext)
        {
            authDbContext = _authDbContext;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var response = new List<ApiResource>();

            var apiResources = await authDbContext.ApiResources
                .Select(x => new ApiResource
                {
                    Name = x.Name,
                    DisplayName= x.DisplayName,
                }).ToListAsync();

            // Mapping API resource to the Response
            foreach (var name in apiResourceNames)
            {
               var apiResource = apiResources.Where(x => x.Name == name).FirstOrDefault();
               if(apiResource != null)
               {
                    response.Add(apiResource);
               }
            }
            return response;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {

            var groupedAPIResoruceScope = await authDbContext.ApiResources
                 .Join(authDbContext.ApiResourceScopes,
                    ar => ar.Id,
                    ars => ars.ApiResourceId,
                    (ar, ars) => new { ApiName = ar.Name, Scope = ars.Scope })
                 .GroupBy(x => x.ApiName)
                 .Select(x => new ApiResource { Name = x.Key, Scopes = x.Select(x => x.Scope).ToList() })
                 .ToArrayAsync();

            // Mapping API resource to the Response with API Scope
            var response = new List<ApiResource>();
            foreach (var name in scopeNames)
            {
                var matchedResource = groupedAPIResoruceScope.Where(x => x.Scopes.Any(s => s == name)).FirstOrDefault();
                if (matchedResource != null)
                {
                    if (!response.Any(x => x.Name == matchedResource.Name))
                    {
                        var apiResource = new ApiResource(matchedResource.Name, new string[] { "user.role", "Age"});
                        apiResource.Scopes = matchedResource.Scopes;
                        response.Add(apiResource);
                    }
                }
            }
            return response;
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var apiScopes = await authDbContext.ApiResourceScopes
                .Select(x => new ApiScope { Name = x.Scope }).ToListAsync();

            // Mapping API Scope to the Response
            var response = new List<ApiScope>();
            foreach (var name in scopeNames)
            {
                var matchedApiScope = apiScopes.Where(x => x.Name == name).FirstOrDefault();
                if (matchedApiScope != null)
                {
                    response.Add(matchedApiScope);
                }
            }
            return response;
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return new List<IdentityResource>
            {
               new IdentityResources.OpenId(),
               new IdentityResources.Profile(),
               new IdentityResource()
               {
                   Name = "apione.claims",
                   UserClaims= { "user.role" }
               }
            };
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var apiScopes = await authDbContext.ApiResourceScopes
                .Select(x => new ApiScope { Name = x.Scope }).ToListAsync();

            var apiResources = await authDbContext.ApiResources
                .Select(x => new ApiResource { Name = x.Name, DisplayName = x.DisplayName }).ToListAsync();

            return new Resources
            {
                ApiResources = apiResources,
                ApiScopes = apiScopes
            };
        }
    }
}
