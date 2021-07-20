using IPGeoLocator.Service;
using System;
using System.Collections.Generic;
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

            public void CreateBatch(Guid batchID,List<string> IPAddresses)
            {
            var newBatch = new Batch()
            {
                BatchId = batchID,
                TimeStamp = DateTime.Now,
                Ipbatches = new List<Ipbatch>()

            };
            IPAddresses.ForEach(ip => newBatch.Ipbatches.Add(new Ipbatch{
                BatchId = batchID,
                Ip = ip }));
                _db.Add(newBatch);
                _db.SaveChanges();
            }

        public void ProcessBatch(Guid BatchID)
        {
            List<Ipbatch> IPsForProcess = new List<Ipbatch>();
            //Geting the IPs that belong to the Batch in a list
            IPsForProcess.AddRange(_db.Batches.SingleOrDefault(b => b.BatchId == BatchID).Ipbatches);
            IPsForProcess.ForEach(ipb => AddIPResultToDB(_ipLocatorService.GetIPDetails(ipb.Ip),ipb.InternalCode));
        }

        private void AddIPResultToDB(IPDetails ipDetails,int ipInternalCode)
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

