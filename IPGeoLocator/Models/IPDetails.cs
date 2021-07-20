using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPGeoLocator.Models
{
    public class IPDetails
    {

        public string IP { get; set; }

        public string CountryCode{ get; set; }

        public string CountryName { get; set; }

        public string TimeZone { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
