using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace OauthServer.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly HttpContext? context;

        public HomeController(IHttpClientFactory httpClientFactory, IHttpContextAccessor accessor)
        {
            httpClient = httpClientFactory.CreateClient();
            context = accessor.HttpContext;
        }
        public async Task<IActionResult> Index()
        {
            
            //var document = await httpClient.GetDiscoveryDocumentAsync("https://localhost:4000/");
            //var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:4000/connect/token",
            //    ClientId = "Client1",
            //    ClientSecret = "Client1Secret",
            //    Scope = "api1.read",
            //});

            //var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            //{
            //    Address = "https://localhost:4000/connect/token",
            //    ClientId = "Client1",
            //    ClientSecret = "Client1Secret",
            //    Scope = "api1.read",
            //    UserName = "user1",
            //    Password = "password",
            //});

            //return Ok(tokenResponse.AccessToken);
            return Ok("Identity Server4 Home Page!!");
        }


        [Authorize]
        public IActionResult Info()
        {
            return Ok("I am Info Page");
        }
    }
}
