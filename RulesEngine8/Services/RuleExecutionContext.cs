namespace RulesEngine8.Services
{
    public class RuleExecutionContext
    {
        public object InputData { get; set; }
        public Dictionary<string, object> State { get; set; } = new Dictionary<string, object>();
    }
}
