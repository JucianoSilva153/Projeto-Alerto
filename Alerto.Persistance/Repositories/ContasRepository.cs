using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Domain.Entities;
using Alerto.Domain.Interfaces;
using Alerto.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Alerto.Persistance.Repositories;

public class ContasRepository(DBToDO acessoDados, ICurrentUser currentUser) : IContaRepository
{
    //TODO: Para poder fazer com que usuarios facam login em suas contas, tenho de primeiro usar autemticacao
    public async Task<RequestResponse> CreateAccount(AdicionarEditarContaDTO conta)
    {
        try
        {
            await acessoDados.Contas.AddAsync(new Conta()
            {
                Nome = conta.Nome,
                email = conta.Email,
                contacto = conta.Contacto,
                password = conta.Password,
                GoogleId = conta.GoogleId
            });

            await acessoDados.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new RequestResponse
            {
                Mensagem = "Erro ao Adicionar Conta",
                Sucesso = false
            };
        }

        return new RequestResponse
        {
            Mensagem = "Conta Adicionada Com Sucesso",
            Sucesso = true,
            Target = conta
        };
    }

    public async Task<RequestResponse> GetAccount()
    {
        try
        {
            var conta = await acessoDados.Contas.FirstOrDefaultAsync(c =>
                c.email == currentUser.Email || c.Id == currentUser.ContaId);

            if (conta is not null)
                return new RequestResponse
                {
                    Mensagem = "Conta encontrada com sucesso!",
                    Sucesso = true,
                    Target = new RetornarContaDTO
                    {
                        Nome = conta.Nome,
                        Contacto = conta.contacto,
                        Email = conta.email,
                        Id = conta.Id
                    }
                };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new RequestResponse
            {
                Mensagem = $"Erro ao encontrar conta!! \n {e.Message} ",
                Sucesso = false
            };
        }

        return new RequestResponse
        {
            Mensagem = "Conta nao encontrada",
            Sucesso = false
        };
    }

    public async Task<RequestResponse> GetAccountStats()
    {
        var tasks = await acessoDados.Tarefas.Where(t => t.ContaId == currentUser.ContaId).ToListAsync();
        var taskLists = await acessoDados.Listas.Where(t => t.ContaId == currentUser.ContaId).ToListAsync();
        var categories = await acessoDados.Categorias.Where(t => t.ContaId == currentUser.ContaId).ToListAsync();
        var tasksDone = await acessoDados.Tarefas.Where(t => t.ContaId == currentUser.ContaId && t.Concluida == true)
            .ToListAsync();

        var stats = new ResumoDTO
        {
            Categoria = categories.Count,
            Concluidas = tasksDone.Count,
            Tarefas = tasks.Count,
            ListasTarefas = taskLists.Count
        };

        return new RequestResponse
        {
            Mensagem = "Resumo retornado com Sucesso!!",
            Sucesso = true,
            Target = stats
        };
    }
}