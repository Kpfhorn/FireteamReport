using DestinyAPI;
using DestinyAPI.DependencyInjection;
using DotNetBungieAPI.DefinitionProvider.Sqlite;
using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace DestinyAPITests
{
    public class DestinyAPITests
    {

        private ITestOutputHelper _output;

        public DestinyAPITests(ITestOutputHelper output)
        {
            _output = output;
        }
        [Theory]
        [InlineData("Kpfhorn")]
        [InlineData("Mara Sov's Throne Cushion")]
        public async Task GetUser(string searchText)
        {
            var destiny2 = ServiceProvider.GetRequiredService<Destiny2>();

            var users = await destiny2.SearchUserAsync(searchText).ToListAsync();

            users.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);

            var user = users[0];

            user.BungieGlobalDisplayName.Should().StartWith(searchText);
        }

        [Theory]
        [InlineData("Kpfhorn")]
        public async Task GetCharacters(string searchText)
        {
            var destiny2 = ServiceProvider.GetRequiredService<Destiny2>();

            var user = await destiny2.SearchUserAsync(searchText).FirstOrDefaultAsync();

            user.Should().NotBeNull();


            var lastCharacter = await user.LastPlayedCharacter;

            lastCharacter.Should().NotBeNull();

            _output.WriteLine("Mobility value is {0}", lastCharacter.Mobility);
            lastCharacter.Mobility.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("Kpfhorn")]
        public async Task GetWeapons(string searchText)
        {
            var destiny2 = ServiceProvider.GetRequiredService<Destiny2>();

            var user = await destiny2.SearchUserAsync(searchText).FirstOrDefaultAsync();

            user.Should().NotBeNull();


            var lastCharacter = await user.LastPlayedCharacter;

            lastCharacter.Should().NotBeNull();

            var weapons = await lastCharacter.EquippedWeapons.ToListAsync();

            weapons.Should().NotBeNull();
        }

        public async Task GetArmor(string searchText) { }


        private IServiceProvider ServiceProvider
        {
            get
            {
                IServiceCollection collection = new ServiceCollection();

                collection.AddLogging();
                collection.AddSerilog(new LoggerConfiguration()
                                        .MinimumLevel.Information()
                                        .MinimumLevel.Override("DestinyAPI.Services.DestinyEquipmentProvider", LogEventLevel.Debug)
                                        .WriteTo.TestOutput(_output)
                                        .WriteTo.Console()
                                        .WriteTo.Debug()
                                        .CreateLogger());

                collection.UseDestinyAPI(apiConfig =>
                {
                    apiConfig.ConfigureBungieApiClient(config =>
                    {
                        config.ClientConfiguration.UsedLocales.Add(DotNetBungieAPI.Models.BungieLocales.EN);
                        config.ClientConfiguration.TryFetchDefinitionsFromProvider = true;
                        config.ClientConfiguration.ApiKey = "59bfc8e734c4470c985dbb225ce6710b";
                        config.DefinitionProvider.UseSqliteDefinitionProvider(sql =>
                        {
                            sql.AutoUpdateManifestOnStartup = true;
                            sql.FetchLatestManifestOnInitialize = true;

                            var manifestPath = Path.GetFullPath(@".\Manifest");
                            if (!Path.Exists(manifestPath))
                            {
                                Directory.CreateDirectory(manifestPath);
                            }

                            sql.ManifestFolderPath = manifestPath;
                        });
                    });
                });

                return collection.BuildServiceProvider();


            }
        }
    }
}