using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SynopticumIdentityServer.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SynopticumIdentityServer.Jwt
{
    public class JwtGenerator(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {

        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var tokenSigningSecretKey = configuration.GetRequiredSection("JWT").GetValue<string>("SecretKey");
            using var sha256 = SHA256.Create();
            var hashedKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(tokenSigningSecretKey));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add user roles as claims
            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(hashedKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "localhost:7000",
                audience: "your_audience_url",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}