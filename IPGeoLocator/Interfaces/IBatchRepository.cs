using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPGeoLocator.Models
{
    public interface IBatchRepository
    {
        public void CreateBatch(Guid BatchID,List<string> IPAddresses);

        public void ProcessBatch(Guid BatchID);

        public IPBatchStatus GetBatchStatus(Guid BatchID);

        public DateTime CalculateRemainingTime(int retrievedBatchIPs, int totalBatchIPs, DateTime? batchStartTimeStamp);
    }
}
