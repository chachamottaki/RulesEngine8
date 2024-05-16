using Microsoft.EntityFrameworkCore;
using Xunit;
using RulesEngine8.Models;

namespace RulesEngine8.Tests
{
    public class RuleEngineTests
    {
        [Fact]
        public async Task TestRuleSerializationAsync()
        {
            var options = new DbContextOptionsBuilder<RulesEngineDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            // Insert test data into the in-memory database
            using (var context = new RulesEngineDBContext(options))
            {
                var rule = new Rule
                {
                    Name = "Test Rule",
                    Description = "This is a test rule",
                    Nodes = new List<string> { "Node1", "Node2" },
                    Conditions = new List<RuleCondition>
                {
                    new RuleCondition { Type = "temperature", Field = "temp", ConditionOperator = ">", Value = "100" }
                },
                    Actions = new List<RuleAction>
                {
                    new RuleAction { Type = "email", Parameters = new Dictionary<string, string> { { "to", "test@example.com" }, { "subject", "Alert" } } }
                }
                };

                context.Rules.Add(rule);
                context.SaveChanges();
            }

            // Retrieve the data to ensure it was inserted correctly
            using (var context = new RulesEngineDBContext(options))
            {
                var rule = await context.Rules.FirstOrDefaultAsync();
                Assert.NotNull(rule);
                Assert.Equal("Test Rule", rule.Name);
                Assert.Single(rule.Conditions);
                Assert.Equal("temperature", rule.Conditions.First().Type);
                Assert.Single(rule.Actions);
                Assert.Equal("email", rule.Actions.First().Type);
                Assert.Equal("test@example.com", rule.Actions.First().Parameters["to"]);
            }
        }
    }
}
