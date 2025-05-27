using Todo.Configuration;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Todo;
using Todo.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// 1. Criar HttpClient temporário para carregar appsettings
var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

// 2. Carregar o JSON
var apiSettings = await http.GetFromJsonAsync<ApiSettings>("appsettings.json");

// 3. Registrar a instância lida
if (apiSettings != null) builder.Services.AddSingleton(apiSettings);

// 4. Registrar HttpClient para APIs
builder.Services.AddScoped(sp => new HttpClient());

// 5. Registrar seus serviços
builder.Services.AddScoped<APIService>();
builder.Services.AddScoped<CookieService>();

await builder.Build().RunAsync();