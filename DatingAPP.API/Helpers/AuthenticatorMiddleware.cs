using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApi.Helpers
{
    public class AuthenticatorMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticatorMiddleware(RequestDelegate next, IHostingEnvironment environment)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { (string)context.Request.Headers["Origin"] });
                context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" });
                context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
                context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
                context.Response.Headers.Add("Access-Control-Max-Age", "1728000");
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("OK");

                return;
            }

             await this._next.Invoke(context);

        }

    }

}

