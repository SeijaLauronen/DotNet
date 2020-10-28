using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harjoitus9_WebAPIHttpClientReact.Models
{
    public class MeasurementByTag
    {

        public DateTimeOffset TimestampISO8601 { get; set; }
  
        public string Tag { get; set; }
        public double Value { get; set; }
    }

}
