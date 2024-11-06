using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SynopticumIdentityServer.Jwt
{
    public class JwtCookieAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtCookieAuthenticationMiddleware(RequestDelegate next, TokenValidationParameters tokenValidationParameters)
        {
            _next = next;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract the JWT from the cookie
            var token = context.Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    // Validate the token using the TokenValidationParameters
                    var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                    
                    // Set the principal (user) in the HttpContext
                    context.User = principal;
                }
                catch (Exception ex)
                {
                    // Token validation failed, so reject the principal (clear any user data)
                    context.User = new ClaimsPrincipal(new ClaimsIdentity());
                    Console.WriteLine($"JWT validation failed: {ex.Message}");
                }
            }

            // Continue with the pipeline
            await _next(context);
        }
    }
}
