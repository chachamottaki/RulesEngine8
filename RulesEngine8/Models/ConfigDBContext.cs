using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RulesEngine8.Models
{
    public class ConfigDBContext : DbContext
    {
        public ConfigDBContext(DbContextOptions<ConfigDBContext> options) : base(options) { }
        public DbSet<ConfigItem> ConfigItems { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
    }

}