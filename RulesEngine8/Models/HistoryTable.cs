namespace RulesEngine8.Models
{
    public class HistoryTable
    {
        public int Id { get; set; }
        public string? assetUUID { get; set; }
        public bool emailSent { get; set; }
        public string? emailContent { get; set; }
        public string? emailRecipient{ get; set; }
        public DateTime timestamp { get; set; }
    }
}
