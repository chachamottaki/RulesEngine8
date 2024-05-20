using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace RulesEngine8.Models
{
    public class RuleChain
    {
        public int RuleChainId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        
        public List<RuleNode> Nodes { get; set; } = new List<RuleNode>();
        
        public string NodesJson
        {
            get => JsonConvert.SerializeObject(Nodes);
            set => Nodes = JsonConvert.DeserializeObject<List<RuleNode>>(value);
        }
    }
}
 