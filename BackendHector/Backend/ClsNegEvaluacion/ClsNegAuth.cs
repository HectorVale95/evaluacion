using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ClbModEvaluacion;
using ClbDatEvaluacion;

namespace ClbNegEvaluacion
{
    public class ClsNegAuth
    {
        private readonly string _connectionString;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpirationMinutes;
        private readonly ClsDatAuth _datAuth;

        public ClsNegAuth(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
            _jwtSecret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret");
            _jwtIssuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
            _jwtAudience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
            _jwtExpirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
            _datAuth = new ClsDatAuth(_connectionString);
        }

        public ClsModAuthResponse Authenticate(ClsModAuth auth)
        {
            try
            {
                var user = _datAuth.ValidateUser(auth.Username, auth.Password);

                if (user == null)
                    throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

                var token = GenerateJwtToken(user);
                return new ClsModAuthResponse
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                    Username = user.Username,
                    Role = user.Role
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la autenticación: {ex.Message}", ex);
            }
        }

        private string GenerateJwtToken(ClsModUsuario user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
