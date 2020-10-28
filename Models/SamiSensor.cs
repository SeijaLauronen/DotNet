using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harjoitus9_WebAPIHttpClientReact.Models
{
    //Täältä ensin kopioitu leikepöydälle JSON näköinen data, sitten paste special, JSON...
    //https://sami.savonia.fi/Service/3.0/MeasurementsService.svc/json/sensors/SK1-tekuEnr34d
    //Firefoxissa ei tullut json näköistä, cromesta sai. Joku asetus jossain?
    public class SamiSensor
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Unit { get; set; }
    }


}
