using Newtonsoft.Json.Linq;

namespace RulesEngine8.Services
{
    public class RuleExecutionContext
    {
        public JObject InputData { get; set; }
        public Dictionary<string, object> State { get; set; } = new Dictionary<string, object>();
        public JObject Result { get; set; }
    }
}
