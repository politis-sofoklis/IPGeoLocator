using System;

namespace IPGeoLocator
{
    public class IPLocatorResponseContract
    {
        public  string ip { get; set; }

        public string country_code { get; set; }

        public string country_name { get; set; }

        public string region_code { get; set; }

        public string region_name { get; set; }

        public string city { get; set; }
        public string zip_code { get; set; }
        public string time_zone { get; set; }

        public decimal latitude { get; set; }

        public decimal longitude { get; set; }
        public int metro_code { get; set; }
    }
}
