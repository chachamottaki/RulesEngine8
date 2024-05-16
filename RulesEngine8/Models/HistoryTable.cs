namespace RulesEngine8.Models
{
    public class HistoryTable
    {
        public int Id { get; set; }
        public bool isAlarm { get; set; }
        public string? alarmId { get; set; }
        public string? assetId { get; set; }
        public string? DeviceId { get; set; }
        public bool emailSent { get; set; }
        public string? emailContent { get; set; }
        public string? emailRecipient{ get; set; }
        public DateTime timestamp { get; set; }
    }
}
