using System.Security.Claims;
using Alerto.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Alerto.Infrastructure.Services;

public class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid ContaId =>
        new Guid(_httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value!);

    public string Email =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? "";

    public string Contacto =>
        _httpContextAccessor.HttpContext?.User?.FindFirst("Contacto")?.Value ?? "";
}