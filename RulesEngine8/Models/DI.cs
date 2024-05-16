using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RulesEngine8.Models
{
    public enum DIType
    {
        Discrete,
        Pulse,
        HighSpeed,
        Analog,
        SpecialFunction,
        Remote
    }
    public enum PushButtonType
    {
        Standard,
        EmergencyStop,
        Selector,
        Illuminated,
        Reset
    }
    public class DI
    {
        public string alarmId { get; set; }
        public bool isActive { get; set; }
        //public ushort? Module { get; set; }
        //public ushort? index { get; set; }
        //public bool RawValue { get; set; }
        //public bool value { get; set; }
        public string? shortDescription { get; set; }
        public string? longDescription { get; set; }
        //public DIType Type { get; set; }
        //public ushort? AlarmLevel { get; set; }
        //public ushort? CurrentAlarmLevel { get; set; }
        public bool isTPST { get; set; }
        public bool sendEmail { get; set; }
        public bool Invert { get; set; }
        //public uint? Color { get; set; }

        //TPST ------------------------
        public bool IsAlarm { get; set; }
        public  string? InstallationType { get; set; }
        public string? InstallationKey { get; set; }
        public string? SensorKey { get; set; }
        public string? SensorType { get; set; }
        public bool SendOnChange { get; set; }
        public bool Send { get; set; }
        public bool Error { get; set; }
        public string? ErrorMsg { get; set; }
        public string? LastTime { get; set; }
                                              

        //public int? Pulses { get; set; }
        //public bool? ResetPulses { get; set; }
        //public TimeSpan DetectionTime { get; set; }
        //public TimeSpan DetectionTimeLeft { get; set; }
        //public bool? Presence { get; set; }
        //public bool? TimeoutDetector { get; set; }
        //public PushButtonType PushButtonType { get; set; }
        //public ushort? AlarmKeyIndex { get; set; }

        public int Id { get; set; }
        public bool topIsActive { get; set; }
        public ushort topIndex { get; set; }
        public ushort DIIndex { get; set; }
        public uint NumChannels { get; set; }
        public string? OrderNumber { get; set; }
        public uint? topColor { get; set; }
        public int ConfigItemID { get; set; }
    }
}
