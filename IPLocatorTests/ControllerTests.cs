using IPGeoLocator.Controllers;
using IPGeoLocator.Models;
using IPGeoLocator.Service;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace IPLocatorTests
{
    public class ControllerTest
    {
        #region Property  
        public Mock<IBatchRepository> mockBatchRepo = new Mock<IBatchRepository>();

        public Mock<IIPLocatorService> mockIPService = new Mock<IIPLocatorService>();
        #endregion

        [Fact]
        public void GetIPDetails()
        {

            string testIP = "8.8.8.8";
            var expectedResult = new IPDetails
            {
                IP = "8.8.8.8",
                CountryCode = "US",
                CountryName = "United States",
                Latitude = 38.7936M,
                Longitude = -90.7854M,
                TimeZone = "America/Chicago"
            };
            mockIPService.Setup(ip => ip.GetIPDetails(testIP)).Returns(expectedResult);
            IPLocatorController bc = new IPLocatorController(mockIPService.Object,mockBatchRepo.Object);
            IPDetails result = bc.Get(testIP);

            Assert.Equal(expectedResult, result);
        }
        [Fact]
        public  void GetBatchStatusSucess()
        {
            Guid testGuid = Guid.Parse("c151f15d-3eb5-421d-b2fb-006508c46e16");
            var expectedResult = new IPBatchStatus
            {
                BatchID = testGuid,
                ProgressReport = "10/10",
                RemainingTime = "30"
            };
            mockBatchRepo.Setup(p => p.GetBatchStatus(testGuid)).Returns(expectedResult);
            IPLocatorBatchController bc = new IPLocatorBatchController(mockBatchRepo.Object);
            IPBatchStatus result =  bc.Get(testGuid);

            Assert.Equal(expectedResult, result);
        }
    }
}
