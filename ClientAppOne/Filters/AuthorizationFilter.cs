using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClientAppOne.Filters
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        List<string> strings = new List<string>() { "/home"};

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string returnUrl = context?.HttpContext?.Request?.Path.ToString();
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string template = controllerActionDescriptor?.AttributeRouteInfo?.Template.ToUpperInvariant();
            string routePath = controllerActionDescriptor?.AttributeRouteInfo?.Template ?? null;


            
            if (!strings.Contains(returnUrl))
            {
                var user = context.HttpContext.User;
                if (user == null)
                {
                    throw new Exception("User Not Found");
                }

                //if (!user.HasClaim(x => x.Type == "user.role" && x.Value == "admin"))
                //{
                //    throw new Exception("Not Allowed");
                //}

            }
        }

    }
}
