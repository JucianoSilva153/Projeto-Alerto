using Alerto.API.ToDo.Services;
using Alerto.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alerto.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificoesController(NotificacaoService notificacoes) : ControllerBase
{
    
}