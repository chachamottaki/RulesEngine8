namespace RulesEngine8.Models
{
    public class ConfigItemModel
    {
        public int Id { get; set; }
        public string UUID { get; set; }
        public string DeviceID { get; set; }
        public string AssetID { get; set; }
        public ConfigJson Config { get; set; }

    }
}
