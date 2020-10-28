using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;//25.10.2020
using Harjoitus9_WebAPIHttpClientReact.Models;

namespace Harjoitus9_WebAPIHttpClientReact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory _httpClientFactory; //25.10.2020

        // 25.10.2020 Dependency Injectionilla IHttpClientFactory
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //KTS https://sami.savonia.fi/Manage/Home/Help#json
        //https://sami.savonia.fi/Service/3.0/MeasurementsService.svc/json/measurements/your-key-here?obj=your-meas-object&tag=your-meas-tag&data-tags=comma-separated-list-of-data-tags&from=from&to=to&take=20&inclusiveFrom=true&inclusiveTo=true&binaryValueFormat=ByteArray
        //https://sami.savonia.fi/Service/3.0/MeasurementsService.svc/json/measurements/SK1-tekuEnr34d?obj=OPI-JKL02&from=2020-01-01&to=2020-01-10
        //Kopioi leikepöydälle tuon datan tulos ja tee uusi luokka Models-kansioon, johon: Paste special, Paste JSON as Classes!!
        //Testaa omassa sovelluksessa: https://localhost:44324/weatherforecast/sami
        [HttpGet("sami")]
        //public async Task<IEnumerable<dynamic>> GetSamiData()
        public async Task<IEnumerable<SamiMeasurement>> GetSamiData()
        {
            HttpClient client = _httpClientFactory.CreateClient("sami");

            var samiApiResponse = await client.GetAsync("SK1-tekuEnr34d?obj=OPI-JKL02&from=2020-01-01&to=2020-01-10");
            if (samiApiResponse.IsSuccessStatusCode)
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<SamiMeasurement>>(await samiApiResponse.Content.ReadAsStringAsync());
                //return System.Text.Json.JsonSerializer.Deserialize<dynamic>(await samiApiResponse.Content.ReadAsStringAsync()); //tämä ei toimi
            }
            Response.StatusCode = (int)samiApiResponse.StatusCode;
            return null;
        }
    }
}
