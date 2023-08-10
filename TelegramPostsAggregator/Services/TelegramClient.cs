using Microsoft.Extensions.Options;
using TelegramPostAggregator.Settings;
using WTelegram;

namespace TelegramPostAggregator.Services
{
    public class TelegramClient 
    {
        private readonly Client client;

        private readonly TelegramClientSettings settings;

        public TelegramClient(Client client, IOptions<TelegramClientSettings> settings)
        {
            this.client = client;
            this.settings = settings.Value;
        }

        //private Client GetTelegramClient()
        //{
        //   // client.LoginUserIfNeeded(settings.PhoneNumber);
        //}

    }
}
