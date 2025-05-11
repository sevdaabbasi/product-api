using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Product.Api.Domain.Interfaces;

namespace Product.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticateAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
            
            if (!context.HttpContext.Request.Headers.TryGetValue("X-Username", out var username) ||
                !context.HttpContext.Request.Headers.TryGetValue("X-Password", out var password))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var isValid = await authService.ValidateUserAsync(username, password);
            if (!isValid)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
} 