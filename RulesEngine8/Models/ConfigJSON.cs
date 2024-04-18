using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.EventLog;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEngine8.Models
{
    public class ConfigJson
    {
        //public string deviceId { get; set; }
        //public string assetId { get; set; }

        //public DeviceSetting? deviceSettings { get; set; }
        //public List<DigitalInput>? digitalInputs { get; set; }
        //public List<DigitalOutput>? digitaloutputs { get; set; }
        //public List<AnalogInput>? analogInputs { get; set; }
        //public List<AnalogOutput>? analogOutputs { get; set; }

        public bool sendEmail { get; set; }
        public string? email { get; set; }
        public string? shortDescription { get; set; }
        public string? longDescription { get; set; }
    }
}
