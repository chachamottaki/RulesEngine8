using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEngine8.Models
{
    public class RuleNode
    {
        public int RuleNodeId { get; set; }
        public string NodeType { get; set; }
        public string ConfigurationJson { get; set; }

        [NotMapped]
        public List<NodeConnection> NodeConnections { get; set; } = new List<NodeConnection>();
        public int RuleChainID { get; set; }
    }
}
