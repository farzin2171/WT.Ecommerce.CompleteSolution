using Microsoft.AspNetCore.Http;

namespace WT.Ecommerce.Infrastructure.Http
{
    public static class HttpContextExtensions
    {
        public static void RedirectToLogout(this HttpContext httpContext)
        {
            httpContext.Response.Redirect(AbsoluteUrl.FromRequest("~/Account/Logout", httpContext.Request));
        }
    }
}
