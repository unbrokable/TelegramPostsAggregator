using TL;
using WTelegram;

namespace TelegramPostAggregator.Services
{
    public class TelegramPostsFetcher
    {
        private readonly Client telegramClient;

        public TelegramPostsFetcher(Client telegramClient)
        {
            this.telegramClient = telegramClient;
        }

        public async Task<IEnumerable<(MessageBase Message, InputPeer sender)>> GetPostsAsync(string channelId, DateTimeOffset? endDate, int maxCount = 10)
        {
            List< (MessageBase Message, InputPeer sender) > messages = new (maxCount);

            InputPeer channelPeer = await telegramClient.Contacts_ResolveUsername(channelId);

            for (int offset_id = 0; ;)
            {
                Messages_MessagesBase offsetMessages = await telegramClient.Messages_GetHistory(channelPeer, offset_id);

                messages.AddRange(offsetMessages
                       .Messages
                       .Select(m => (m, GetMessageBaseInputPeer(offsetMessages, m))));

                if(!offsetMessages.Messages.Any() || offsetMessages.Messages.Any(m => m.Date < endDate) || offsetMessages.Count >= maxCount)
                {
                    return messages
                          .Where(m => endDate is null ? true : m.Message.Date >= endDate)
                          .Take(maxCount);
                }

                offset_id = offsetMessages.Messages[^1].ID;
            }
        }

        private InputPeer GetMessageBaseInputPeer(Messages_MessagesBase messagesBase, MessageBase message)
        {
            return messagesBase.UserOrChat(message.From ?? message.Peer).ToInputPeer();
        }
    }
}
