
using DotNetBungieAPI;
using DotNetBungieAPI.DefinitionProvider.Sqlite;
using DotNetBungieAPI.Extensions;
using FireteamReport.Components;
using Serilog;
using Microsoft.Data.Sqlite;
using Microsoft.FluentUI.AspNetCore.Components;
using DestinyAPI.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.UseDestinyAPI(apiConfig => {

    apiConfig.FetchLatestManifestOnInitialize = true;

    apiConfig.ConfigureBungieApiClient(config => {
        config.ClientConfiguration.UsedLocales.Add(DotNetBungieAPI.Models.BungieLocales.EN);
        config.ClientConfiguration.TryFetchDefinitionsFromProvider = true;
        config.ClientConfiguration.ApiKey = "59bfc8e734c4470c985dbb225ce6710b";
        config.DefinitionProvider.UseSqliteDefinitionProvider(sql =>
        {
            sql.AutoUpdateManifestOnStartup = true;
            sql.FetchLatestManifestOnInitialize = true;

            var manifestPath = Path.GetFullPath(@"./Manifest");
            if (!Path.Exists(manifestPath))
            {
                Directory.CreateDirectory(manifestPath);
            }

            sql.ManifestFolderPath = manifestPath;
        });
    });
});

//Add Bungie API service to the container
// builder.Services.UseBungieApiClient(config =>
// {
//     config.ClientConfiguration.UsedLocales.Add(DotNetBungieAPI.Models.BungieLocales.EN);
//     config.ClientConfiguration.TryFetchDefinitionsFromProvider = true;
//     config.ClientConfiguration.ApiKey = "59bfc8e734c4470c985dbb225ce6710b";
//     config.DefinitionProvider.UseSqliteDefinitionProvider(sql =>
//     {
//         sql.AutoUpdateManifestOnStartup = true;
//         sql.FetchLatestManifestOnInitialize = true;

//         var manifestPath = Path.GetFullPath(@".\Manifest");
//         if (!Path.Exists(manifestPath))
//         {
//             Directory.CreateDirectory(manifestPath);
//         }

//         sql.ManifestFolderPath = manifestPath;
//     });
// });

//Add Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddFluentUIComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();