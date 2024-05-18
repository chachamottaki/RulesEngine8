using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEngine8.Models
{
    public class Rule
    {
        public int RuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<String> Nodes { get; set; }

        [NotMapped] // Do not map this property to the database
        public List<RuleCondition> Conditions { get; set; }

        // This property is mapped to the database
        public string ConditionsJson
        {
            get => JsonConvert.SerializeObject(Conditions, Formatting.None,
                       new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            set => Conditions = JsonConvert.DeserializeObject<List<RuleCondition>>(value);
        }

        [NotMapped] // Do not map this property to the database
        public List<RuleAction> Actions { get; set; }

        // This property is mapped to the database
        public string ActionsJson
        {
            get => JsonConvert.SerializeObject(Actions, Formatting.None,
                       new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            set => Actions = JsonConvert.DeserializeObject<List<RuleAction>>(value);
        }

    }
}
