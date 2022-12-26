using AirQuality.Models.Dtos;
using AirQuality.Models.Entities;
using AutoMapper;

namespace AirQuality.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SensorsDto, SensorsModel>();
        }
    }
}
