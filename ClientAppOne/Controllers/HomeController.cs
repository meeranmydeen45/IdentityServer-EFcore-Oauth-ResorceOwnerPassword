using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ClientAppOne.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        
        private readonly IHttpClientFactory clientFactory;
        private readonly HttpContext? context;
        public HomeController(IHttpClientFactory httpClientFactory, IHttpContextAccessor accessor)
        {
            clientFactory = httpClientFactory;
            context = accessor.HttpContext;
        }

        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HomePage()
        {
            var user = context?.User;
            return Ok("Client Home Page 1");
        }

        //[Authorize(Policy = "userPolicy")]
        [HttpGet("adminPage")]
        public async Task<IActionResult> AdminPage()
        {
            var accessToken = await context.GetTokenAsync("access_token");
           // var idToken = await context.GetTokenAsync("id_token");

            var claims = context?.User.Claims;
            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            //var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            return Ok("AdminPage");
        }

        //[Authorize(Policy = "clientPolicy")]
        [HttpGet("clientPage")]
        public async Task<IActionResult> ClientPage()
        {
            var user = context?.User;
            return Ok("ClientPage");
        }
    }
}