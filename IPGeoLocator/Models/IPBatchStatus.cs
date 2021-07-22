using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPGeoLocator.Models
{
    public class IPBatchStatus
    {
        public Guid BatchID { get; set; }

        public string ProgressReport { get; set; }

        public string RemainingTime { get; set; }
    }
}
