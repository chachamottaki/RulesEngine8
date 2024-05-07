using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace RulesEngine8.Models
{
    public class RulesEngineDBContext : DbContext
    {
        public RulesEngineDBContext(DbContextOptions<RulesEngineDBContext> options) : base(options) { }
        public DbSet<ConfigItem> ConfigItems { get; set; }
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
        }
    }
}