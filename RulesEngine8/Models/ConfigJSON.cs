using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.EventLog;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEngine8.Models
{
    public class ConfigJson
    {
        public string? email { get; set; }
    }
}
