using System;
using System.Threading.Tasks;
using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Domain.Interfaces;

namespace Alerto.Application.Services;

public class ContaService(IContaRepository contaRepository)
{
    public async Task<RequestResponse> NovaContaAsync(AdicionarEditarContaDTO novaConta)
    {
        try
        {
            return await contaRepository.CreateAccount(novaConta);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar criar conta: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar criar conta!!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> RetornarContaAtualAsync()
    {
        try
        {
            return await contaRepository.GetAccount();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar encontrar conta: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar encontrar conta!!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> RetornarResumoContaAsync()
    {
        try
        {
            return await contaRepository.GetAccountStats();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar retornar resumo: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar retornar resumo!!",
            Sucesso = false
        };
    }
}