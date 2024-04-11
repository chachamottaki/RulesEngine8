using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RulesEngine8.Models
{
    public class RulesEngineDBContext : DbContext
    {
        public RulesEngineDBContext(DbContextOptions<RulesEngineDBContext> options) : base(options) { }
        public DbSet<ConfigItemModel> ConfigItems { get; set; }
        public DbSet<SensorModel> Sensors { get; set; }
    }

}