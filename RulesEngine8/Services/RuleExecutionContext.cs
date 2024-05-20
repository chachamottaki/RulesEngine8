using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace RulesEngine8.Services
{
    public class RuleExecutionContext
    {
        public int RuleChainId { get; set; }
        public JsonObject InputData { get; set; }
        public Dictionary<string, object> State { get; set; } = new Dictionary<string, object>();
        public JsonObject Result { get; set; }
        public string ListeningEndpoint { get; set; }
    }
}
