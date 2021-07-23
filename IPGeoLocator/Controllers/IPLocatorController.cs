using Hangfire;
using IPGeoLocator.Models;
using IPGeoLocator.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IPGeoLocator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IPLocatorController : ControllerBase
    {

        private readonly IIPLocatorService _ipLocatorService;

        private readonly IBatchRepository _repo;



        public IPLocatorController( IIPLocatorService ipLocatorService , IBatchRepository repo)
        {
            _ipLocatorService = ipLocatorService;
            _repo = repo;
        }

        [HttpGet]
        public IPDetails Get(string IPAdress)
        {
            return _ipLocatorService.GetIPDetails(IPAdress);
        }
        [HttpPost]
        public  string Post(List<string> IPAdresses)
        {
            Guid batchID = Guid.NewGuid();
            var createJob = BackgroundJob.Enqueue(
                () => _repo.CreateBatch(batchID, IPAdresses));
            BackgroundJob.ContinueJobWith(
                createJob,
            () => _repo.ProcessBatch(batchID));
            string batchURL = $"/IPLocatorBatch?BatchID={batchID}";
            return batchURL;

        }
    }
}
