using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harjoitus9_WebAPIHttpClientReact.Models
{
    public class SamiMeasurement
    {
        public Datum[] Data { get; set; }
        public string Object { get; set; }
        public string Tag { get; set; }
        public DateTimeOffset TimestampISO8601 { get; set; }
    }

    public class Datum
    {
        public string Tag { get; set; }
        public double Value { get; set; }
    }
}

