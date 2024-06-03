using System.Net;

namespace WebApplication5
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                await context.Response.WriteAsync("access denied");
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                await context.Response.WriteAsync("not found");
            }
        }
    }
}
