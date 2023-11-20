using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientTwoApp.Controllers
{
    public class HomeController : ControllerBase
    {
        [Authorize]
        public IActionResult Get()
        {
            var res = User.Claims;
            return Ok("Client App Two");
        }
    }
}