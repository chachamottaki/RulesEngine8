﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RulesEngine8.Models
{
    public class RuleNode
    {
        public int RuleNodeId { get; set; }
        public string NodeUUID { get; set; }
        public string NodeType { get; set; }
        public string ConfigurationJson { get; set; }

        [NotMapped]
        public List<NodeConnection> NodeConnections { get; set; } = new List<NodeConnection>();
        public int RuleChainID { get; set; }

        //properties for positioning on canvas
        public int Left { get; set; }
        public int Top { get; set; }
    }
}
