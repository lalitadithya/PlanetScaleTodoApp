using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(PlanetScaleTodoApp.Functions.Startup))]
namespace PlanetScaleTodoApp.Functions
{
    public class Startup : FunctionsStartup
    {
        private static IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton((s) => {
                string endpoint = configuration["EndPointUrl"];
                string authKey = configuration["AuthorizationKey"];
                CosmosClientBuilder configurationBuilder = new CosmosClientBuilder(endpoint, authKey);
                return configurationBuilder.Build();
            });
        }
    }
}
