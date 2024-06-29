using API.Context;
using API.ToDo.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.ToDo.Services;

public class ContasService
{
    private DBToDO acessoDados;

    public ContasService(DBToDO acessoDados)
    {
        this.acessoDados = acessoDados;
    }


    //TODO: Para poder fazer com que usuarios facam login em suas contas, tenho de primeiro usar autemticacao
    public async Task<RequestResponse> CreateAccount(AdicionarEditarContaDTO conta)
    {
        try
        {
            await acessoDados.Conta.AddAsync(new ContaModel()
            {
                Nome = conta.Nome,
                email = conta.Email,
                contacto = conta.Contacto,
                password = conta.Password
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

    public async Task<RequestResponse> GetAccountByEmail(string email)
    {
        try
        {
            var conta = await acessoDados.Conta.FirstOrDefaultAsync(c => c.email == email);

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
}