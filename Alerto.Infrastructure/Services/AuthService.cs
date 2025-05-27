using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Alerto.Common.DTO;
using Alerto.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Alerto.Infrastructure.Services;

public class AuthService
{
    private readonly DBToDO dataAcess;
    private IConfiguration config;
    public AuthService(DBToDO dataAcess, IConfiguration config)
    {
        this.dataAcess = dataAcess;
        this.config = config;
    }

    public async Task<string> GerarToken(AutenticarContaDTO conta)
    {
        var usuario = await dataAcess.Contas
            .FirstOrDefaultAsync(c => c.email == conta.Email && c.password == conta.Password);
       
        if (usuario is not null)
        {
            var Issuer = config["JWT:issuer"];
            var Audience = config["JWT:audience"];
            var SecretKey = Encoding.UTF32.GetBytes(config["JWT:key"]);

            Console.WriteLine($"Issuer: {Issuer}, Audience: {Audience}, Key: {config["JWT:key"]}");
            
            var tokenDecriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("Id", usuario.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, conta.Email),
                    new Claim("Contacto", usuario.contacto.ToString())
                }),
                Audience = Audience,
                Issuer = Issuer,
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(SecretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDecriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
        
        return String.Empty;
    }
    
    public async Task<string> GerarTokenViaGoogle(string email, string googleId)
    {
        var usuario = await dataAcess.Contas
            .FirstOrDefaultAsync(c => c.email == email && c.GoogleId == googleId);
       
        if (usuario is not null)
        {
            var Issuer = config["JWT:issuer"];
            var Audience = config["JWT:audience"];
            var SecretKey = Encoding.UTF32.GetBytes(config["JWT:key"]);

            Console.WriteLine($"Issuer: {Issuer}, Audience: {Audience}, Key: {config["JWT:key"]}");
            
            var tokenDecriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("Id", usuario.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim("Contacto", usuario.contacto.ToString())
                }),
                Audience = Audience,
                Issuer = Issuer,
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(SecretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDecriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
        
        return String.Empty;
    }
}