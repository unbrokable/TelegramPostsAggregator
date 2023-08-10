using TelegramPostAggregator.Services.Tasks;
using TelegramPostAggregator.Services;
using WTelegram;
using TelegramPostAggregator.Settings;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    private static TelegramClientSettings telegramClientSettings = new ();

    private static TelegramImporterSettings telegramImporterSettings = new();

    public static IServiceCollection AddTelegramServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramImporterSettings>(
            configuration.GetSection(nameof(TelegramImporterSettings)));

        configuration.GetSection(nameof(TelegramClientSettings)).Bind(telegramClientSettings);

        services.AddSingleton<Client>(_ =>
        {
            Client client = new(telegramClientSettings.ApiId, telegramClientSettings.ApiHash);

            string response = client.Login(telegramClientSettings.PhoneNumber).Result;

            return client;
        });

        services.AddSingleton<TelegramPostsFetcher>();
        services.AddSingleton<TelegramPostsImporter>();
        services.AddSingleton<TelegramChannelPostUpdateListener>();

        return services;
    }
}

