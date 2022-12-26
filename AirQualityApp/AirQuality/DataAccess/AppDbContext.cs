using AirQuality.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirQuality.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<SensorsModel> SensorsData { get; set; }
        public DbSet<LogsModel> Logs { get; set; }

    }
}
