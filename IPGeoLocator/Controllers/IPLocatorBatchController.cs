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
    public class IPLocatorBatchController : ControllerBase
    {

        private readonly ILogger<IPLocatorBatchController> _logger;


        private readonly IBatchRepository _repo;



        public IPLocatorBatchController(ILogger<IPLocatorBatchController> logger, IBatchRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public IPBatchStatus Get(Guid BatchID)
        {
            return _repo.GetBatchStatus(BatchID);
        }
    }
}
