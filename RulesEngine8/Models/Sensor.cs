namespace RulesEngine8.Models
{
    public class Sensor
    {
        public bool status { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime timestampLocal { get; set; }
        public string description { get; set; }
        public bool isAlarm { get; set; }
        public string alarmStatus { get; set; }
        public string alarmId { get; set; }
    }
}
