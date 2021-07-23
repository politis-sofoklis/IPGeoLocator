using IPGeoLocator.Models;
using IPGeoLocator.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace IPGeoLocator.Models
{
    public class BatchRepository : IBatchRepository
    {

        private readonly IPLocatorDBContext _db;

        private readonly IIPLocatorService _ipLocatorService;
        public BatchRepository(IPLocatorDBContext db, IIPLocatorService ipLocatorService)
        {
            _db = db;
            _ipLocatorService = ipLocatorService;
        }

        public IQueryable<Batch> Batches => _db.Batches;

        public IQueryable<Ipbatch> IpBatches => _db.Ipbatches;

        public void CreateBatch(Guid batchID, List<string> IPAddresses)
        {
            var newBatch = new Batch()
            {
                BatchId = batchID,
                TimeStamp = DateTime.Now,
                Ipbatches = new List<Ipbatch>()

            };
            IPAddresses.ForEach(ip => newBatch.Ipbatches.Add(new Ipbatch
            {
                BatchId = batchID,
                Ip = ip,
                IsRetrieved = false
            }));
            _db.Add(newBatch);
            _db.SaveChanges();
        }

        public void ProcessBatch(Guid BatchID)
        {

            List<Ipbatch> IPsForProcess = new List<Ipbatch>();
            //Geting the IPs that belong to the Batch in a list
            var ipsOfBatch = _db.Ipbatches.Where(b => b.BatchId == BatchID);
            if(ipsOfBatch == null)
            {
                return;
            }
            IPsForProcess.AddRange(ipsOfBatch);
            //Getting the details for each IP
            foreach (var ipb in IPsForProcess)
            {
                var details = _ipLocatorService.GetIPDetails(ipb.Ip);
                AddIPResultToDB(details, ipb.InternalCode);
            }
        }

        private void AddIPResultToDB(IPDetails ipDetails, int ipInternalCode)
        {
            Ipbatch ipbatch = _db.Ipbatches.SingleOrDefault(ipb => ipb.InternalCode == ipInternalCode);
            if(ipbatch == null)
            {
                return;
            }
            ipbatch.Ip = ipDetails.IP;
            ipbatch.CountryCode = ipDetails.CountryCode;
            ipbatch.CountryName = ipDetails.CountryName;
            ipbatch.TimeZone = ipDetails.TimeZone;
            ipbatch.Latitude = ipDetails.Latitude.ToString();
            ipbatch.Longitude = ipDetails.Longitude.ToString();
            ipbatch.IsRetrieved = true;
            _db.Update(ipbatch);
            _db.SaveChanges();
        }

        public IPBatchStatus GetBatchStatus(Guid BatchID)
        {
            var totalBatchIPdetails = IpBatches.Where(ipb => ipb.BatchId == BatchID);
            int totalBatchIPs = totalBatchIPdetails.Count();
            int retrievedBatchIPs = totalBatchIPdetails.Count(ipb => ipb.IsRetrieved == true);
            var batchStartTimeStamp = Batches.SingleOrDefault(b => b.BatchId == BatchID).TimeStamp;
            var batchStatus = new IPBatchStatus
            {
                BatchID = BatchID,

            };
            if (totalBatchIPdetails == null || totalBatchIPs ==0 || batchStartTimeStamp == null)
            {
                batchStatus.ProgressReport = "Error with calculating batch status";
                batchStatus.RemainingTime = "Error with calculating remaining time";
                return batchStatus;
            }
            batchStatus.ProgressReport = $"{retrievedBatchIPs}/{totalBatchIPs}";
            if (retrievedBatchIPs == 0)
            {
                batchStatus.RemainingTime = $"IPs not yet retrived.Cannot calculate remaining time.";
            }
            else
            {
                DateTime remainingTime = CalculateRemainingTime(retrievedBatchIPs, totalBatchIPs, batchStartTimeStamp);
                batchStatus.RemainingTime = $"{remainingTime.Hour} h, {remainingTime.Minute} m, {remainingTime.Second} s, {remainingTime.Millisecond} ms.";
            }
            return batchStatus;
        }

        public  DateTime  CalculateRemainingTime (int retrievedBatchIPs,int totalBatchIPs, DateTime? batchStartTimeStamp)
        {
            int elapsedTimeInMs = Convert.ToInt32((DateTime.Now - batchStartTimeStamp).Value.TotalMilliseconds);
            var remainingMs = (elapsedTimeInMs * (totalBatchIPs -retrievedBatchIPs)) / retrievedBatchIPs;
            var remainingTime = new DateTime();
            remainingTime = remainingTime.AddMilliseconds(remainingMs);
            return remainingTime;
        }
    }
}

