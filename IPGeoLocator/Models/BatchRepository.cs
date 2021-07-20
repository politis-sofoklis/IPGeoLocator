using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPGeoLocator.Models
{
    public class BatchRepository : IBatchRepository
    {

            private IPLocatorDBContext db;

            public BatchRepository(IPLocatorDBContext db)
            {
                this.db = db;
            }

            public IQueryable<Batch> Batches => db.Batches;

            public Guid CreateBatch(List<string> IPAddresses)
            {
            Guid batchGuid =  Guid.NewGuid();
            var newBatch = new Batch()
            {
                BatchId = batchGuid,
                TimeStamp = DateTime.Now,
                Ipbatches = new List<Ipbatch>()

            };
            IPAddresses.ForEach(ip => newBatch.Ipbatches.Add(new Ipbatch{
                BatchId = batchGuid,
                Ip = ip }));
                db.Add(newBatch);
                db.SaveChanges();
                return batchGuid;
        }

        }
    }

