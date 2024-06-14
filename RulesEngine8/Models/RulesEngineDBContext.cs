using Microsoft.EntityFrameworkCore;

namespace RulesEngine8.Models
{
    public class RulesEngineDBContext : DbContext
    {
        public RulesEngineDBContext(DbContextOptions<RulesEngineDBContext> options) : base(options) { }
        public DbSet<ConfigItem> ConfigItems { get; set; }
        public DbSet<DI> DigitalInputs { get; set; }
        public DbSet<HistoryTable> HistoryTables { get; set; }
        public DbSet<RuleChain> RuleChains { get; set; }
        public DbSet<RuleNode> RuleNodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigItem>()
                .OwnsOne(configItem => configItem.Config, builder => { builder.ToJson(); });

            modelBuilder.Entity<DI>()
                .HasOne<ConfigItem>()
                .WithMany(c => c.DigitalInputs)
                .HasForeignKey(di => di.ConfigItemID)
                .IsRequired();

            // Configure NodesJson as text for PostgreSQL
            modelBuilder.Entity<RuleChain>()
                .Property(rc => rc.NodesJson)
                .HasColumnType("text");
            modelBuilder.Entity<RuleChain>()
                .Property(rc => rc.IsActive)
                .HasDefaultValue(false);

            modelBuilder.Entity<RuleNode>()
                .HasOne<RuleChain>()
                .WithMany(c => c.Nodes)
                .HasForeignKey(rn => rn.RuleChainID)
                .IsRequired();

            modelBuilder.Ignore<NodeConnection>();
        }
    }
}
