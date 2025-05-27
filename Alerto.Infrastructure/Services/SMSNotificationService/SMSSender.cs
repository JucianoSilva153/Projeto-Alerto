using System.Net.Http.Json;
using Alerto.API.ToDo.Services.OmbalaService.Models;
using Microsoft.Extensions.Configuration;

namespace Alerto.API.ToDo.Services;

public class SMSSender : OMBALA, ISMSSender
{
    public string Sender { get; set; }

    public async Task<bool> SendMessage(Message msg)
    {
        try
        {
            var result = await ombalaHttpClient.PostAsJsonAsync("/v1/messages", msg);
            if (result.IsSuccessStatusCode)
                return true;
            Console.WriteLine("Erro ao enviar mensagem: " + result.Content);
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro ao enviar mensagem: " + e.Message);
        }

        return false;
    }

    public SMSSender(IConfiguration settings) : base(settings)
    {
        Sender = settings["Ombala:Sender"] ?? "";
    }
}