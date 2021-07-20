using System;
using System.Collections.Generic;

#nullable disable

namespace IPGeoLocator.Models
{
    public partial class Batch
    {
        public Batch()
        {
            Ipbatches = new HashSet<Ipbatch>();
        }

        public Guid BatchId { get; set; }
        public DateTime? TimeStamp { get; set; }

        public virtual ICollection<Ipbatch> Ipbatches { get; set; }
    }
}
