using AirQuality.Models;
using AirQuality.Models.Dtos;
using AirQuality.Models.Entities;
using AirQuality.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace AirAPI.MVC.Controllers
{
    public class SensorController : Controller
    {
        private readonly ISensorsService _sensorService;

        public SensorController(ISensorsService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpGet("/")]
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("/currentQuality")]
        public async Task<ActionResult> GetCurrentQuality()
        {
            try
            {
                var result = await _sensorService.GetCurrentQuality();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/saveData")]
        public async Task Create([FromQuery] SensorsModel data)
        {
            await _sensorService.SaveData(data);
        }

        [HttpGet("/1234/seedSampleData")]
        public async Task<ActionResult> SeedSampleData()
        {
            try
            {
                await _sensorService.SeedSampleData();

                return Redirect("/");
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpGet("/1234/clearSensorsTable")]
        public async Task<ActionResult> ClearSensorsTable()
        {
            try
            {
                await _sensorService.ClearSensorsTable();

                return Redirect("/");
            }
            catch (Exception ex)
            {
                return View("Error", "Something went wrong");
            }
        }

        [HttpGet("/sensorsData")]
        public ActionResult GetAllData([FromQuery] int timeSpan)
        {

            var results = _sensorService.GetData(TimeSpan.FromHours(timeSpan + 1));

            return Ok(results);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
