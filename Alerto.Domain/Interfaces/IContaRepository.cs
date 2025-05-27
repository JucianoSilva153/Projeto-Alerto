using Alerto.Common.Abstractions;
using Alerto.Common.DTO;

namespace Alerto.Domain.Interfaces;

public interface IContaRepository
{
    public Task<RequestResponse> CreateAccount(AdicionarEditarContaDTO conta);
    public Task<RequestResponse> GetAccount();
    public Task<RequestResponse> GetAccountStats();
}