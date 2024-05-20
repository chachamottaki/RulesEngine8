using RulesEngine8.Models;
using RulesEngine8.Services;
using System.Threading.Tasks;

namespace RulesEngine8.Processors
{
    public interface IRuleNodeProcessor
    {
        string NodeType { get; }
        Task ProcessAsync(RuleNode node, RuleExecutionContext context);
    }
}
