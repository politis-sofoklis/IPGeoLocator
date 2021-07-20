﻿using IPGeoLocator.Models;
using IPGeoLocator.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public Guid Post(List<string> IPAdresses)
        {

            return _repo.CreateBatch(IPAdresses); ;
        }
    }
}
