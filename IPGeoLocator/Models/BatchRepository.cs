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

        public  void ProcessBatch(Guid BatchID)
        {

            List<Ipbatch> IPsForProcess = new List<Ipbatch>();
            //Geting the IPs that belong to the Batch in a list
            IPsForProcess.AddRange(_db.Ipbatches.Where(b => b.BatchId == BatchID));
            //Getting the details for each IP
            foreach (var ipb in IPsForProcess)
            {
                var details = _ipLocatorService.GetIPDetails(ipb.Ip);
                AddIPResultToDB(details,ipb.InternalCode);
            }
        }

        private void AddIPResultToDB(IPDetails ipDetails, int ipInternalCode)
        {
            Ipbatch ipbatch = _db.Ipbatches.SingleOrDefault(ipb => ipb.InternalCode == ipInternalCode);
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
    }
}

