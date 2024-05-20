using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            get => JsonSerializer.Serialize(Nodes);
            set => Nodes = JsonSerializer.Deserialize<List<RuleNode>>(value);
        }
    }
}
