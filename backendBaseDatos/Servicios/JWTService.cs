using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backendBaseDatos.Servicios
{
    public class JWTService
    {
        public static string SecretKey = "5b8dabee936e7e1d74539e001a52b859d514355488b523d3084a5f6945a05035e54e0f3ee93afcf3d6fcbca393418df8a1d56bb2ac860b7c170bcec68a869235";
        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));

        public static JwtSecurityToken GenerateToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: "Bases de Datos",
                audience: "UCU",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha512)
            );
            return token;
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
