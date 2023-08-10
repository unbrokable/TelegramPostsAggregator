using TL;
using WTelegram;

namespace TelegramPostAggregator.Services.Tasks
{
    public class TelegramChannelPostUpdateListener
    {
        private readonly Client telegramClient;

        private readonly Dictionary<long, User> Users = new ();

        private readonly Dictionary<long, ChatBase> Chats = new ();

        public TelegramChannelPostUpdateListener(Client telegramClient)
        {
            this.telegramClient = telegramClient;

            Messages_Dialogs dialogs = telegramClient.Messages_GetAllDialogs().Result;
            
            dialogs.CollectUsersChats(Users, Chats);
        }

        public async Task Start()
        {
            telegramClient.OnUpdate += async updates =>
            {
                foreach (Update post in updates.UpdateList) {

                    switch (post)
                    {
                        case UpdateNewMessage unm: await HandleNewMessage(unm.message); break;
                    }

                    Console.WriteLine(post.ToString());
                
                }
            };

            while (true)
            {
                await Task.Delay(2000);
            }
        }

        private  Task HandleNewMessage(MessageBase messageBase)
		{

            switch (messageBase)
			{
				case Message m: Console.WriteLine($"Message info. Author: {GetPeerName(messageBase.From)} in Chat: {GetPeerName(m.peer_id)} Message: {m.message}"); break;
			}

			return Task.CompletedTask;
		}

        string GetPeerName(Peer peer)
        {
            return peer switch
            {
                PeerChannel or PeerChat => GetChannelString(peer.ID),
                PeerUser user => GetUserString(user.ID),
                _ => "Not set peer type",
            };
        }

        string GetChannelString(long channelId)
        {
            if (Chats.TryGetValue(channelId, out ChatBase? chat))
            {
                return chat?.ToString() ?? $"Chat {channelId}";
            }

            return $"Chat {channelId}";
        }

        string GetUserString(long userId)
        {
            if (Users.TryGetValue(userId, out User? user))
            {
                return user?.ToString() ?? $"User {userId}";
            }
            return $"User {userId}";
        }
    }
}
