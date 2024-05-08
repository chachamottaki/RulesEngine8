using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.EventLog;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEngine8.Models
{
    public class ConfigJson
    {
        public bool sendEmail { get; set; } // this should be removed bc xMail is from Di/DO/...
        public string? email { get; set; }
        
    }
}
