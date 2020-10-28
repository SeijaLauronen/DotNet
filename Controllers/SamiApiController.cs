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
    public class SamiApiController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SamiApiController> _logger;
        private readonly IHttpClientFactory _httpClientFactory; //25.10.2020

        // 25.10.2020 Dependency Injectionilla IHttpClientFactory
        public SamiApiController(ILogger<SamiApiController> logger, IHttpClientFactory httpClientFactory)
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
        //Testaa omassa sovelluksessa: https://localhost:44324/weatherforecast/sami --> muutettu:
        //Testaa omassa sovelluksessa: https://localhost:44324/samiapi/measurements
        [HttpGet("measurements")]
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

        [HttpGet("measurements/{id}")] //TODO kyssärillä voi yhdistää nämä molemmat
        //public async Task<IEnumerable<dynamic>> GetSamiData()
        //public async Task<IEnumerable<SamiMeasurement>> GetSamiData(string id)
        public async Task<IEnumerable<MeasurementByTag>> GetSamiData(string id)
        {
            HttpClient client = _httpClientFactory.CreateClient("sami");

            var samiApiResponse = await client.GetAsync("SK1-tekuEnr34d?obj=OPI-JKL02&from=2020-01-01&to=2020-01-10&data-tags="+ id);//27.10.2020 &tag= --> &data-tags=
            if (samiApiResponse.IsSuccessStatusCode)
            {
                var retval= System.Text.Json.JsonSerializer.Deserialize<List<SamiMeasurement>>(await samiApiResponse.Content.ReadAsStringAsync());

                var retlista = new List<MeasurementByTag>();
                //TODO TÄHÄN LISTA NOITA:
                int i = 1;
                foreach (SamiMeasurement sm in retval) {
                    MeasurementByTag retvalSimple = new MeasurementByTag();
                    retvalSimple.TimestampISO8601 = sm.TimestampISO8601;
                    retvalSimple.Tag = sm.Data[0].Tag; //Tämä ei mikään hyvä konsti...
                    retvalSimple.Value = sm.Data[0].Value;
                    retlista.Add(retvalSimple);
                    //Tämä siksi, että tuli joku virhe myöhemmin...
                    i++;
                    if (i > 5) { 
                        break; 
                    }
                }


                return retlista;
                //return System.Text.Json.JsonSerializer.Deserialize<List<SamiMeasurement>>(await samiApiResponse.Content.ReadAsStringAsync());//Tämä ok, palauttaa koko measurement-objektin
                //return System.Text.Json.JsonSerializer.Deserialize<dynamic>(await samiApiResponse.Content.ReadAsStringAsync()); //tämä ei toimi
            }
            Response.StatusCode = (int)samiApiResponse.StatusCode;
            return null;
        }

        //https://sami.savonia.fi/Service/3.0/MeasurementsService.svc/json/sensors/SK1-tekuEnr34d
        //Testaa omassa sovelluksessa: https://localhost:44324/samiapi/sensors
        [HttpGet("sensors")]
        public async Task<IEnumerable<SamiSensor>> GetSamiSensors()
        {
            HttpClient client = _httpClientFactory.CreateClient("samisensors");

            var samiApiResponse = await client.GetAsync("SK1-tekuEnr34d"); //TODO: avain jonnekin esim appsettings?
            if (samiApiResponse.IsSuccessStatusCode)
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<SamiSensor>>(await samiApiResponse.Content.ReadAsStringAsync());     
            }
            Response.StatusCode = (int)samiApiResponse.StatusCode;
            return null;
        }
        /*
         * <script src="http://code.jquery.com/jquery-2.1.4.min.js"></script>
<script>
$(function () {
    // get sensors
    var getSensors = function () {
        var key = 'your-read-key';
        var url = 'https://sami.savonia.fi/Service/3.0/MeasurementsService.svc/json/sensors/' + key;
        $.getJSON(url, null, function (data) {
            var sensorInfo = JSON.stringify(data, null, 4));
        });
    };
});
</script>
        */



    }
}
