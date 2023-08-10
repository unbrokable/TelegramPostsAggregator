namespace TelegramPostAggregator.Settings
{
    public class TelegramClientSettings
    {
        public string PhoneNumber { get; set; } = string.Empty;
        
        public int ApiId { get; set; }

        public string ApiHash { get; set; } = string.Empty;
    }
}
