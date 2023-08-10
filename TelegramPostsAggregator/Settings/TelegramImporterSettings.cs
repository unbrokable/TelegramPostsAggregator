namespace TelegramPostAggregator.Settings
{
    public class TelegramImporterSettings
    {
        public string ChannelDestination { get; set; } = string.Empty;

        public IEnumerable<string> ImportedChannels { get; set; } = new List<string>();
    }
}
