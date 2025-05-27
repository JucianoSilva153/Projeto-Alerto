

using Alerto.API.ToDo.Services.OmbalaService.Models;

namespace Alerto.API.ToDo.Services;

public interface ISMSSender
{
    public Task<bool> SendMessage(Message msg);
}