using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace Alerto.API.ToDo.Services.OmbalaService.Models;

public class OMBALA
{
    public HttpClient ombalaHttpClient = new HttpClient();

    public OMBALA(IConfiguration settings)
    {
        ombalaHttpClient.BaseAddress = new Uri(settings["Ombala:BaseAdress"] ?? "");
        ombalaHttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Token", settings["Ombala:Token"]);
    }
}