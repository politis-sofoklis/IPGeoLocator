using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using IPGeoLocator.Service;
using IPGeoLocator.Models;

namespace IPGeoLocator.Service
{
    public class IPLocatorService: IIPLocatorService
    {
        private static readonly string _uri = "https://freegeoip.app/json/";

        private static HttpClient client;

        public IPLocatorService()
        {
            if (client == null)
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                client = new HttpClient(handler);
            }
        }


        public IPDetails GetIPDetails(string IPAdress)
        {
            HttpResponseMessage response = client.GetAsync(_uri + IPAdress).Result;

            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
            var deserializedResponse = JsonSerializer.Deserialize<IPLocatorResponseContract>(result);
            return TransformIPResponseContractToIPDetails(deserializedResponse);
        }

        private static IPDetails TransformIPResponseContractToIPDetails(IPLocatorResponseContract response)
        {
            return new IPDetails()
            {
                IP = response.ip,
                CountryCode = response.country_code,
                CountryName = response.country_name,
                TimeZone = response.time_zone,
                Latitude = response.latitude,
                Longitude = response.longitude
            };
        }
    }
}

