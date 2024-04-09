using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RuleEngine.Models
{
    public class ConfigDBContext : DbContext
    {
        public ConfigDBContext(DbContextOptions<ConfigDBContext> options) : base(options) { }
        public DbSet<ConfigItem> ConfigItems { get; set; }
    }
}