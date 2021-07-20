using IPGeoLocator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPGeoLocator.Service
{
    public interface IIPLocatorService
    {
        IPDetails GetIPDetails(string IPAdress);
    }
}
