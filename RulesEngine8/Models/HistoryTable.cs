namespace RulesEngine8.Models
{
    public class HistoryTable
    {
        public int Id { get; set; }
        public bool emailSent { get; set; }
        public string? emailContent{ get; set; }
        public DateTime timestamp { get; set; }
        public DateTime timestampLocal { get; set; }
    }
}
