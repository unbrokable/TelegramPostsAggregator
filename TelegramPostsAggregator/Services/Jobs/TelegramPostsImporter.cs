using Microsoft.Extensions.Options;
using TelegramPostAggregator.Settings;
using TL;
using WTelegram;

namespace TelegramPostAggregator.Services.Tasks
{
    public class TelegramPostsImporter
    {
        private readonly Client telegramClient;

        private readonly TelegramPostsFetcher fetcher;

        private readonly IEnumerable<string> channels;

        private readonly string channelDestination;

        public TelegramPostsImporter(Client telegramClient, TelegramPostsFetcher fetcher, IOptions<TelegramImporterSettings> options)
        {
            this.telegramClient = telegramClient;
            //telegramClient.LoginUserIfNeeded( ).Wait();

            this.fetcher = fetcher;
            this.channels = options.Value.ImportedChannels;
            this.channelDestination = options.Value.ChannelDestination;
        }

        public async Task Import()
        {
            InputPeer channel = await telegramClient.Contacts_ResolveUsername(channelDestination);

            DateTimeOffset? lastDate = await GetLastPostDate(channel);

            List<(MessageBase Message, InputPeer Sender)> messages = new();

            foreach (string channelId in channels)
            {
                IEnumerable<(MessageBase Message, InputPeer Sender)> channelMessages = 
                    await fetcher.GetPostsAsync(channelId, lastDate);

                messages.AddRange(channelMessages);
            }

            foreach ((MessageBase message, InputPeer sender) in messages.OrderBy(m => m.Message.Date))
            {
                await telegramClient
                    .Messages_ForwardMessages(sender, new[] { message.ID }, new[] { WTelegram.Helpers.RandomLong() }, channel);
            }
        }

        private async Task<DateTimeOffset?> GetLastPostDate(InputPeer channel)
        {
            Messages_MessagesBase offsetMessages = await telegramClient.Messages_GetHistory(channel, limit: 1);

            MessageBase? message = offsetMessages.Messages.FirstOrDefault();

            return message is null ? null : message.Date;
        }
    }
}
