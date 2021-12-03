using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BluePrint.Client.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using BluePrint.Client.AuthProviders;

namespace BluePrint.Client
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
          //  await DebugDelayAsync();

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("BluePrint.ServerAPI", client =>
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOidcAuthentication(options =>
            {
                // Replace the Okta placeholders with your Okta values in the appsettings.json file.
                options.ProviderOptions.Authority = builder.Configuration.GetValue<string>("Okta:Authority");
                options.ProviderOptions.ClientId = builder.Configuration.GetValue<string>("Okta:ClientId");
                options.ProviderOptions.ResponseType = "code";
            });

            builder.Services.AddScoped<StudentService>();

            builder.Services.AddApiAuthorization();

            // register the Telerik services
            builder.Services.AddTelerikBlazor();

            await builder.Build().RunAsync();
        }

        private static async Task DebugDelayAsync()
        {
#if DEBUG
            await Task.Delay(5000);
#endif
        }
        //public static async Task Main(string[] args)
        //{
        //    var builder = WebAssemblyHostBuilder.CreateDefault(args);
        //    builder.RootComponents.Add<App>("#app");

        //    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        //    await builder.Build().RunAsync();
        //}
    }
}
