namespace RulesEngine8.Models
{
    public class ConfigItemModel
    {
        public int Id { get; set; }
        public string? UUID { get; set; }
        public string? DeviceID { get; set; }
        public string? AssetID { get; set; }
        public ConfigJson? Config { get; set; }
        public List<DI>? digitalInputs { get; set; }
        public List<DO>? digitalOutputs { get; set; }

        //public List<AI> analogInputs { get; set; }
        //public List<AO> analogOutputs { get; set; }
        //public List<MbusMeter> mbusMeters { get; set; }
        //public List<Alarm> alarms { get; set; }

    }
}
