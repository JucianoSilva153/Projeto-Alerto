﻿using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Todo.Configuration;
using Todo.Models;
using Todo.Models.DTO;

namespace Todo.Services;

public class APIService
{
    private readonly HttpClient client;
    private readonly CookieService cookies;
    private readonly ApiSettings settings;

    public ContaLogadaDTO conta = new ContaLogadaDTO();
    public string BaseURI => settings.BaseUri;
    public string BaseGoogleAuthURI => $"{BaseURI}/api/conta/autenticar-google";

    private double DuracaoCookie = 5.0 / (24.0 * 60.0); // 5 minutos

    public APIService(CookieService cookies, ApiSettings settings)
    {
        this.client = new HttpClient();
        this.cookies = cookies;
        this.settings = settings;
    }

    public async Task<bool> Autenticado()
    {
        Console.Write(BaseURI);
        await AtribuiTokenAoHeader();
        try
        {
            var response = await client.GetAsync($"{BaseURI}/api/Conta/Autenticado");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }

    public async Task<ResumoDTO> RetornarResumo()
    {
        await AtribuiTokenAoHeader();
        try
        {
            var response = await client.GetFromJsonAsync<RequestResponse>(
                $"{BaseURI}/api/Conta/Resumo"
            );

            if (response.Sucesso)
            {
                try
                {
                    var resumo = JsonConvert.DeserializeObject<ResumoDTO>(response.Target.ToString());

                    if (resumo is not null)
                        return resumo;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return new ResumoDTO();
        }

        return new ResumoDTO();
    }

    public async Task<bool> Autenticar(string Email, string Password)
    {
        try
        {
            var credenciais = new { email = Email, password = Password };

            var response = await client.PostAsJsonAsync(
                $"{BaseURI}/api/conta/autenticar",
                credenciais
            );

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadFromJsonAsync<RequestResponse>();

                if (resultado.Sucesso)
                {
                    await cookies.CriarCookie("token", resultado.Target.ToString(), DuracaoCookie);
                    await AtribuiTokenAoHeader();

                    if (await RetornarContaPeloEmail(Email))
                        return true;
                }
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return false;
    }

    public async Task<bool> AutenticarViaGoogle()
    {
        try
        {
            var response = await client.GetAsync(
                $"{BaseURI}/api/conta/login-google"
            );

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadFromJsonAsync<RequestResponse>();

                if (resultado.Sucesso)
                {
                    await cookies.CriarCookie("token", resultado.Target.ToString(), DuracaoCookie);
                    await AtribuiTokenAoHeader();

                    return true;
                }
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return false;
    }

    private async Task<bool> RetornarContaPeloEmail(string email)
    {
        AtribuiTokenAoHeader();
        try
        {
            var response = await client.GetFromJsonAsync<RequestResponse>(
                $"{BaseURI}/api/Conta/PorEmail/{email}"
            );

            if (response.Sucesso)
            {
                try
                {
                    conta = JsonConvert.DeserializeObject<ContaLogadaDTO>(response.Target.ToString());

                    if (conta is not null)
                        return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return false;
    }

    private async Task AtribuiTokenAoHeader()
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            await cookies.RetornarCookie("token")
        );

        Console.WriteLine(await cookies.RetornarCookie("token"));
    }

    public async Task<List<ListaAlteraListaTarefaDTO>> ListarListas()
    {
        try
        {
            var response = await client.GetFromJsonAsync<RequestResponse>($"{BaseURI}/api/Listas");
            if (response.Sucesso)
            {
                var resultado =
                    JsonConvert.DeserializeObject<List<ListaAlteraListaTarefaDTO>>(response.Target.ToString());
                if (resultado is not null)
                    return resultado;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return new List<ListaAlteraListaTarefaDTO>();
    }

    public async Task<List<ListaAlteraCategorias>> ListarCategorias()
    {
        try
        {
            var response = await client.GetFromJsonAsync<RequestResponse>($"{BaseURI}/api/Categorias");

            if (response.Sucesso)
            {
                var lista = JsonConvert.DeserializeObject<List<ListaAlteraCategorias>>(response.Target.ToString());
                if (lista is not null)
                    return lista;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return new List<ListaAlteraCategorias>();
    }

    public async Task<List<ListaTarefaDTO>> ListarTarefas()
    {
        try
        {
            var response = await client.GetFromJsonAsync<RequestResponse>($"{BaseURI}/api/Tarefas");

            if (response.Sucesso)
            {
                var tarefas = JsonConvert.DeserializeObject<List<ListaTarefaDTO>>(response.Target.ToString());
                if (tarefas is not null)
                    return tarefas;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return new List<ListaTarefaDTO>();
    }

    public async Task<bool> CriarConta(AdicionarEditarContaDTO conta)
    {
        try
        {
            var response = await client.PostAsJsonAsync<AdicionarEditarContaDTO>(
                $"{BaseURI}/api/Conta",
                conta
            );

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return true;
    }

    public async Task<bool> CriarTarefa(CriaTarefaDTO tarefa)
    {
        try
        {
            var response = await client.PostAsJsonAsync<CriaTarefaDTO>($"{BaseURI}/api/Tarefas", tarefa);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }

    public async Task<bool> ConcluirTarefa(int IdTarefa)
    {
        try
        {
            var response = await client.PutAsync($"{BaseURI}/api/Tarefas/Concluir/{IdTarefa}", null);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return false;
    }

    public async Task<bool> CriarCategoria(CriaCategoriaDTO categoria)
    {
        try
        {
            var response = await client.PostAsJsonAsync<CriaCategoriaDTO>($"{BaseURI}/api/Categorias", categoria);

            if (response.IsSuccessStatusCode)
            {
                var sucesso = await response.Content.ReadFromJsonAsync<RequestResponse>();
                if (sucesso.Sucesso)
                    return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return false;
    }

    public async Task<bool> EliminarCategoria(int IdCategoria)
    {
        try
        {
            var response = await client.DeleteFromJsonAsync<RequestResponse>($"{BaseURI}/api/Categorias/{IdCategoria}");
            if (response.Sucesso)
                return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return false;
    }

    public async Task<bool> CriarListaDeTarefa(string NomeLista)
    {
        try
        {
            var response = await client.PostAsJsonAsync<CriaListaTarefasDTO>($"{BaseURI}/api/Listas",
                new CriaListaTarefasDTO()
                {
                    Lista = NomeLista,
                    ContaID = conta.Id
                });

            if (response.IsSuccessStatusCode)
                return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return false;
    }
}