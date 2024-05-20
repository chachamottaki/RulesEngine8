using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RulesEngine8.Models
{
    public class RuleChain
    {
        public int RuleChainId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [NotMapped]
        public List<RuleNode> Nodes { get; set; } = new List<RuleNode>();

        public string NodesJson
        {
            get => JsonSerializer.Serialize(Nodes);
            set => Nodes = string.IsNullOrEmpty(value) ? new List<RuleNode>() : JsonSerializer.Deserialize<List<RuleNode>>(value);
        }
    }
}
