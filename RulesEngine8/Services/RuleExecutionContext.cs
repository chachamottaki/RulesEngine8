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
        public HashSet<int> ProcessedNodes { get; set; } = new HashSet<int>();
    }
}
