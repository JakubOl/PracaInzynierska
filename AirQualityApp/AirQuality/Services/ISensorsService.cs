using AirQuality.Models.Dtos;
using AirQuality.Models.Entities;

namespace AirQuality.Services
{
    public interface ISensorsService
    {
        Task SaveData(SensorsModel data);
        List<SensorsModel> GetData(TimeSpan timeSpan);
        Task<SensorsModel> GetCurrentQuality();
        Task ClearSensorsTable();
        Task SeedSampleData();
    }
}