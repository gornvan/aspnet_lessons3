namespace FriendsManager.MVC.Security.CsrfCrutchHeaderMiddleware
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class SecurityHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityHeaderMiddleware> _logger;

        public SecurityHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context).ConfigureAwait(false);
                return;
            }

            // Check if the required header is present
            if (
                !context.Request.Headers.TryGetValue(
                    SecurityConstants.SecurityCrutchHeaderName, out var headerValue)
                || headerValue != "CRUTCH"
            )
            {
                // Deny the request by returning a 403 Forbidden response
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(
                    $"Forbidden: Missing or invalid security header " +
                    $"'{SecurityConstants.SecurityCrutchHeaderName}'.");
                await context.Response.CompleteAsync().ConfigureAwait(false);
                return;
            }

            // Call the next middleware if header is present and valid
            await _next(context);
        }
    }

}
