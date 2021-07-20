using System;
using System.Collections.Generic;

#nullable disable

namespace IPGeoLocator.Models
{
    public partial class Ipbatch
    {
        public Guid? BatchId { get; set; }
        public string Ip { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string TimeZone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int InternalCode { get; set; }
        public bool? IsRetrieved { get; set; }

        public virtual Batch Batch { get; set; }
    }
}
