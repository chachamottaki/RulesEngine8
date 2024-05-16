using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEngine8.Models
{
    public class ConfigItem
    {
        public int Id { get; set; }
        public string? DeviceID { get; set; }
        public string? AssetID { get; set; }
        public ConfigJson? Config { get; set; } 

        public List<DI> DigitalInputs { get; set; } 

        public string DigitalInputsJson
        {
            get => JsonConvert.SerializeObject(DigitalInputs);
            set => DigitalInputs = JsonConvert.DeserializeObject<List<DI>>(value);
        }

        //public List<DO>? digitalOutputs { get; set; }
        //public List<AI> analogInputs { get; set; }
        //public List<AO> analogOutputs { get; set; }
        //public List<MbusMeter> mbusMeters { get; set; }
        //public List<Alarm> alarms { get; set; }

    }
}
