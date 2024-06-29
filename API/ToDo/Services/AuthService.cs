using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Context;
using API.ToDo.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.ToDo.Services;

public class AuthService
{
    private readonly DBToDO dataAcess;
    private WebApplicationBuilder builder;
    public AuthService(DBToDO dataAcess)
    {
        this.dataAcess = dataAcess;
        this.builder = WebApplication.CreateBuilder();
    }

    public async Task<string> GerarToken(AutenticarContaDTO conta)
    {
        var usuario = await dataAcess.Conta
            .FirstOrDefaultAsync(c => c.email == conta.Email && c.password == conta.Password);
        if (usuario is not null)
        {
            var Issuer = builder.Configuration["JWT:issuer"];
            var Audience = builder.Configuration["JWT:audience"];
            var SecretKey = Encoding.UTF32.GetBytes(builder.Configuration["JWT:key"]);

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
}