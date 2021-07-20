using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPGeoLocator.Models
{
    public interface IBatchRepository
    {
        public Guid CreateBatch(List<string> IPAddresses);
    }
}
