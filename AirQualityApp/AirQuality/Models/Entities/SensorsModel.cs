using System.ComponentModel.DataAnnotations;

namespace AirQuality.Models.Entities
{
    public class SensorsModel
    {
        [Key]
        public int Id { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Ozone { get; set; }
        public double PM_1_0 { get; set; }
        public double PM_2_5 { get; set; }
        public double PM_10_0 { get; set; }
        public double TVOC { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow + TimeSpan.FromHours(1);
    }
}
