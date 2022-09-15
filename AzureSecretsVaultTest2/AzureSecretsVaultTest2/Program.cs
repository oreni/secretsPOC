using Amazon.Runtime;
using Azure.Identity;
using AzureSecretsVaultTest2.Infrastructure;
using AzureSecretsVaultTest2.InfraStructure;
using AzureSecretsVaultTest2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Runtime;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Azure vault Setup -> can be replaced with AWS vault by replace this part only, all the DI and the rest dont change.
//var azureVaultSettings = builder.Configuration.GetSection("KeyVault").Get<AzureVaultSettings>();
//if (azureVaultSettings.UseVault)// when not used, will read secrets from local secrets file.
//{
//    builder.Configuration.AddAzureKeyVault(new AzureKeyVaultConfigurationOptions(
//           $"https://{azureVaultSettings.VaultName}.vault.azure.net/",
//           azureVaultSettings.ApplicationClientId,
//           azureVaultSettings.SecretValue )
//        {
//            ReloadInterval = TimeSpan.FromSeconds(azureVaultSettings.ReloadIntervalSeconds),
//            Manager = new MyAzureKeyVaultManager(azureVaultSettings.KeyNamePrefix)
//        });
//}
// Azure vault setup
// AWS Secrets manager
var awsVaultSettings = builder.Configuration.GetSection("SecretManager").Get<AzureVaultSettings>();
string project = "Poc";
string Prefix = "Shared";
string env = "Development";
string sharedPrefix = $"{env}_{project}_{Prefix}_";
string servicePrefix = $"{env}_{project}_{awsVaultSettings.KeyNamePrefix}_";
if (awsVaultSettings.UseVault)// when not used, will read secrets from local secrets file.
{
    var awsCredentials = new BasicAWSCredentials(awsVaultSettings.ApplicationClientId, awsVaultSettings.SecretValue);
    builder.Configuration.AddSecretsManager(credentials: awsCredentials, region: Amazon.RegionEndpoint.EUWest1,
    configurator: options =>
    {
        options.SecretFilter = entry => entry.Name.StartsWith($"{awsVaultSettings.KeyNamePrefix}") || entry.Name.StartsWith($"{sharedPrefix}");
        options.KeyGenerator = (_, s) => s.Replace($"{sharedPrefix}", String.Empty).Replace($"{awsVaultSettings.KeyNamePrefix}", String.Empty).Replace("__", ":");
        options.PollingInterval = TimeSpan.FromSeconds(awsVaultSettings.ReloadIntervalSeconds);
    });
}

// Add services to the container.
builder.Services.AddTransient<IStartupFilter, SettingValidationStartupFilter>();


builder.Services.Configure<SalesForceSettings>(builder.Configuration.GetSection(nameof(SalesForceSettings)));// link object to configuration
builder.Services.AddScoped(resolver =>
        resolver.GetRequiredService<IOptionsSnapshot<SalesForceSettings>>().Value);// link object injection to option snapshot so it gets updated values
builder.Services.AddSingleton<IValidatable>(resolver =>
      resolver.GetRequiredService<IOptions<SalesForceSettings>>().Value);

builder.Services.Configure<OktaSettings>(builder.Configuration.GetSection(nameof(OktaSettings)));// link object to configuration

builder.Services.AddScoped(resolver => resolver.GetRequiredService<IOptionsSnapshot<OktaSettings>>().Value);// link object injection to option snapshot so it gets updated values
builder.Services.AddSingleton<IValidatable>(resolver =>
      resolver.GetRequiredService<IOptions<OktaSettings>>().Value);

builder.Services.AddScoped<IOktaClient, OktaClient>();// create scoped objects like this to have them updated with the configuration objets.

builder.Services.AddHttpClient<IOktaClientWHttp, OktaClientWHttp>().ConfigureHttpClient((services,client) =>
    {
        using (var scope = services.CreateScope())
        {
            var scopedProvider = scope.ServiceProvider;
            var test = scopedProvider.GetRequiredService<OktaSettings>();

            client.BaseAddress = new Uri(test.Domain);
        }
    }
);

//add SQL Server SCR Farm
builder.Services.AddDbContext<DbContext>( (services, options) =>
{
    options.UseSqlServer(services.GetRequiredService<OktaSettings>().Domain);
});

builder.Services.AddHttpClient("AccountService", (services, client) =>
{    
    using (var scope = services.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        var test = scopedProvider.GetRequiredService<OktaSettings>();

        client.BaseAddress = new Uri(test.Domain);
        client.DefaultRequestHeaders.Add("AccessKey", test.ClientId);
    }
});


// NOT TO DO => Will make the object not update with the rest of the config objects
var OktaSettingsBad = builder.Configuration.GetSection("OktaSettings").Get<OktaSettings>();
OktaClientBad badClient = new OktaClientBad(OktaSettingsBad);
builder.Services.AddScoped<IOktaClientBad, OktaClientBad>(x=>badClient);
//-------------------------------------------------------------------------------

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
