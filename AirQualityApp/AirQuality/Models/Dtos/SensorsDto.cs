namespace AirQuality.Models.Dtos
{
    public class SensorsDto
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Ozone { get; set; }
        public double PM_1_0 { get; set; }
        public double PM_2_5 { get; set; }
        public double PM_10_0 { get; set; }
        public double TVOC { get; set; }
    }
}
