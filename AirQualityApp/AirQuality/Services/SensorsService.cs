using AirQuality.DataAccess;
using AirQuality.Models.Dtos;
using AirQuality.Models.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirQuality.Services
{
    public class SensorsService : ISensorsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SensorsService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SensorsModel> GetCurrentQuality()
        {
            try
            {
                var data = _context
                            .SensorsData
                            //.Where(s => s.ApiID == apiID)
                            .OrderByDescending(s => s.ReceivedAt)
                            .FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                await SaveLog(ex);

                throw new Exception(ex.Message);
            }
        }

        public async Task SaveData(SensorsModel data)
        {
            try
            {
                var result = await _context.SensorsData.AddAsync(data);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await SaveLog(ex);
            }
        }

        public List<SensorsModel> GetData(TimeSpan timeSpan)
        {
            var now = DateTime.UtcNow + TimeSpan.FromHours(1);

            var data = _context
                        .SensorsData
                        .Where(s => s.ReceivedAt > now - timeSpan)
                        .OrderBy(s => s.ReceivedAt)
                        .ToList();

            return CountAveragePerHour(data);
        }

        public List<SensorsModel> CountAveragePerHour(List<SensorsModel> input)
        {
            return input.GroupBy(s => s.ReceivedAt.Day.ToString() + " " + s.ReceivedAt.Hour.ToString())
                .Select(g =>
                    new SensorsModel()
                    {
                        Humidity = Math.Round(g.Average(s => s.Humidity),1),
                        Temperature = Math.Round(g.Average(s => s.Temperature),1),
                        Ozone = Math.Round(g.Average(s => s.Ozone), 1),
                        PM_10_0 = Math.Round(g.Average(s => s.PM_10_0), 1),
                        PM_1_0 = Math.Round(g.Average(s => s.PM_1_0), 1),
                        PM_2_5 = Math.Round(g.Average(s => s.PM_2_5), 1),
                        TVOC = Math.Round(g.Average(s => s.TVOC), 1),
                        ReceivedAt = g.First().ReceivedAt,
                    })
                .ToList();
        }

        public async Task SeedSampleData()
        {
            try
            {
                var random = new Random();

                for (int i = 0; i < 100; i++)
                {
                    var sensorsData = new SensorsModel()
                    {
                        Humidity = random.Next(0, 10),
                        Temperature = random.Next(0, 10),
                        Ozone = random.Next(0, 10),
                        PM_10_0 = random.Next(0, 10),
                        PM_1_0 = random.Next(0, 10),
                        PM_2_5 = random.Next(0, 10),
                        TVOC = random.Next(0, 10),
                        ReceivedAt = DateTime.Now - TimeSpan.FromHours(i),
                    };
                    var result = await _context.SensorsData.AddAsync(sensorsData);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await SaveLog(ex);

                throw new Exception(ex.Message);
            }
        }

        public async Task ClearSensorsTable()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM SensorsData");
            }
            catch (Exception ex)
            {
                await SaveLog(ex);

                throw new Exception(ex.Message);
            }
        }




        //private int isValidApiKey(string ApiKey)
        //{
        //    var apiKey = _context.ApiKeys.FirstOrDefault(a => a.Key == ApiKey);

        //    if (apiKey == null)
        //    {
        //        throw new Exception("Invalid Api Key");
        //    }

        //    return apiKey.Id;
        //}
        private async Task SaveLog(Exception ex)
        {
            var log = new LogsModel
            {
                Message = ex.Message
            };

            await _context.Logs.AddAsync(log);
        }
    }
}
