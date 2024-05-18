using Newtonsoft.Json;

namespace RulesEngine8.Models
{
    public class RuleAction
    {
        public string Type { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
        public string Json
        {
            get => JsonConvert.SerializeObject(Parameters);
            set => Parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
        }
    }
}
