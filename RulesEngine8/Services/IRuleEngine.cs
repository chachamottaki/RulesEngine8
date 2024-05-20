using System.Threading.Tasks;

namespace RulesEngine8.Services
{
    public interface IRuleEngine
    {
        Task ExecuteRuleChain(int ruleChainId, RuleExecutionContext context);
    }
}
