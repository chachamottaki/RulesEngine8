using System.Threading.Tasks;
using RulesEngine8.Models;

namespace RulesEngine8.Services
{
    public interface IRuleNodeProcessor
    {
        string NodeType { get; }
        Task ProcessAsync(RuleNode node, RuleExecutionContext context);
    }
}


