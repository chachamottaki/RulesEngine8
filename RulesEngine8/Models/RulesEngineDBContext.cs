using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace RulesEngine8.Models
{
    public class RulesEngineDBContext : DbContext
    {
        public RulesEngineDBContext(DbContextOptions<RulesEngineDBContext> options) : base(options) { }
        public DbSet<ConfigItem> ConfigItems { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<DI> DigitalInputs { get; set; }
        public DbSet<SensorModel> Sensors { get; set; }
        public DbSet<HistoryTable> HistoryTables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigItem>()
                .OwnsOne(configItem => configItem.Config, builder => { builder.ToJson(); });

            modelBuilder.Entity<DI>()
                .HasOne<ConfigItem>() // DI has one ConfigItem
                .WithMany(c => c.DigitalInputs) // ConfigItem has many DigitalInputs
                .HasForeignKey(di => di.ConfigItemID) // Foreign key relationship
                .IsRequired(); // DigitalInputs are required

            modelBuilder.Entity<Rule>()
                .Property(r => r.ConditionsJson)
                .HasConversion(
                    v => JsonConvert.SerializeObject(JsonConvert.DeserializeObject<List<RuleCondition>>(v), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.SerializeObject(JsonConvert.DeserializeObject<List<RuleCondition>>(v), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                )
                .IsUnicode(false)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<Rule>()
                .Property(r => r.ActionsJson)
                .HasConversion(
                    v => JsonConvert.SerializeObject(JsonConvert.DeserializeObject<List<RuleAction>>(v), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.SerializeObject(JsonConvert.DeserializeObject<List<RuleAction>>(v), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                )
                .IsUnicode(false)
                .HasColumnType("nvarchar(max)");
        }
    }
}