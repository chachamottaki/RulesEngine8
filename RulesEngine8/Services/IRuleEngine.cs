namespace RulesEngine8.Services
{
    public interface IRuleEngine
    {
        Task ExecuteRuleChain(int ruleChainId, object inputData);
    }
}
