using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GroceriesWasmApp.Client.Services;
using GroceriesWasmApp.Shared;

namespace GroceriesWasmApp.Client {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("GroceriesWasmApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GroceriesWasmApp.ServerAPI"));

            builder.Services.AddMsalAuthentication(options => {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://groceriesb2c.onmicrosoft.com/1a9edbbe-db91-4b4d-9c56-80a52d60762c/API.Access");
                options.ProviderOptions.LoginMode = "redirect";
            });


            //builder.Services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
            builder.Services.AddScoped<ICosmosConnector, ServerStorage>();
            builder.Services.AddScoped<StateService>();

            await builder.Build().RunAsync();
        }
    }
}
