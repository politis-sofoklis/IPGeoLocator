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

        private readonly ILogger<IPLocatorController> _logger;

        private readonly IIPLocatorService _ipLocatorService;

        private readonly IBatchRepository _repo;



        public IPLocatorController(ILogger<IPLocatorController> logger , IIPLocatorService ipLocatorService , IBatchRepository repo)
        {
            _logger = logger;
            _ipLocatorService = ipLocatorService;
            _repo = repo;
        }

        [HttpGet]
        public IPDetails Get(string IPAdress)
        {
            return _ipLocatorService.GetIPDetails(IPAdress);
        }
        [HttpPost]
        public   Guid Post(List<string> IPAdresses)
        {
            Guid batchID = Guid.NewGuid();
            var createJob = BackgroundJob.Enqueue(
                () => _repo.CreateBatch(batchID, IPAdresses));
            BackgroundJob.ContinueJobWith(
                createJob,
            () => _repo.ProcessBatch(batchID));
            return batchID;

        }
    }
}
