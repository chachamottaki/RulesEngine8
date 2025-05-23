﻿namespace RulesEngine8.Models
{
    public class SensorModel
    {
        public int Id { get; set; }
        public bool status { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime timestampLocal { get; set; }
        public string description { get; set; }
        public bool isAlarm { get; set; }
        public string alarmStatus { get; set; }
        public string alarmId { get; set; }
    }
}
