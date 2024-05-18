using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace RulesEngine8.Models
{
    public class RulesEngineDBContext : DbContext
    {
        public RulesEngineDBContext(DbContextOptions<RulesEngineDBContext> options) : base(options) { }
        public DbSet<ConfigItem> ConfigItems { get; set; }
        public DbSet<DI> DigitalInputs { get; set; }
        public DbSet<SensorModel> Sensors { get; set; }
        public DbSet<HistoryTable> HistoryTables { get; set; }

        public DbSet<RuleChain> RuleChains { get; set; }
        public DbSet<RuleNode> RuleNodes { get; set; }
        public DbSet<NodeConnection> NodeConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigItem>()
                .OwnsOne(configItem => configItem.Config, builder => { builder.ToJson(); });

            modelBuilder.Entity<DI>()
                .HasOne<ConfigItem>()
                .WithMany(c => c.DigitalInputs)
                .HasForeignKey(di => di.ConfigItemID)
                .IsRequired();

            // Configure NodesJson as a nvarchar(max) for SQL Server
            modelBuilder.Entity<RuleChain>()
                .Property(rc => rc.NodesJson)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<RuleNode>()
                .HasOne<RuleChain>()
                .WithMany(c => c.Nodes)
                .HasForeignKey(rn => rn.RuleChainID)
                .IsRequired();

            modelBuilder.Ignore<NodeConnection>();



            //modelBuilder.Entity<RuleChain>()
            //.Property(rc => rc.NodesJson)
            //.HasConversion(
            // v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            //v => JsonSerializer.Deserialize<string>(v, (JsonSerializerOptions)null))
            //.HasColumnType("json");

            //modelBuilder.Ignore<NodeConnection>(); // Ignore NodeConnection entity
            //modelBuilder.Ignore<RuleNode>(); // Ignore NodeConnection entity
        }
    }
}
