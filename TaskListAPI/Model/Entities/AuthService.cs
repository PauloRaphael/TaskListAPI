// TaskListAPI/Services/AuthService.cs
using TaskListAPI.Model.Entities;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System;

namespace TaskListAPI.Services
{
    // O IConfiguration é injetado para acessar a chave secreta do JWT no appsettings.json
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Lê a chave secreta. **ATENÇÃO**: Deve ser longa e secreta.
            var secret = _configuration["Jwt:Key"]
                         ?? throw new InvalidOperationException("A chave JWT (Jwt:Key) não está configurada em appsettings.json.");

            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claims (informações do usuário)
                Subject = new ClaimsIdentity(new[]
                {
                    // Adicionamos o ID e o Email do usuário ao token
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Nome)
                }),
                // Define o tempo de expiração do token (Ex: 2 horas)
                Expires = DateTime.UtcNow.AddHours(2),

                // Credenciais de assinatura para validar o token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}