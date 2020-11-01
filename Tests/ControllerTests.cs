using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using RoutingAssistant.BusinessLayer.Contracts;
using RoutingAssistant.BusinessLayer.Dtos;
using RoutingAssistant.BusinessLayer.Implementations;
using RoutingAssistant.Controllers;
using RoutingAssistant.Core.Configuration;
using RoutingAssistant.DataLayer;
using RoutingAssistant.DataLayer.Contracts;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Route_Controller_Should
    {
        private IOsmRouter _osmRouter;
        private Mock<ITourRepository> _tourRepositoryMock;
        private Mock<IArrivalTimeService> _arrivalTimeServiceMock;

        private IMappingService _mappingService;
        public Route_Controller_Should()
        {
            var configuration = Options.Create(new TravelServiceOptions
            {
                RouterDBPath = @"D:\OSM",
                RouterDBFileName = "file.routerdb"
            });
            _osmRouter = new OsmRouter(configuration);

            _tourRepositoryMock = new Mock<ITourRepository>();
            _arrivalTimeServiceMock = new Mock<IArrivalTimeService>();
            _mappingService = new MappingService();

        }

          [Fact]
        public async Task Construct_TestRoute_1()
        {
            //Arragen
            var travelService = new TravelService(_osmRouter);
            var routeConstructionService = new RouteConstructionService(travelService,_mappingService, _tourRepositoryMock.Object);
            var controller = new RouteController(routeConstructionService, _arrivalTimeServiceMock.Object);
            
            //Act
            var response = await controller.CreateRoute(JsonConvert.DeserializeObject<RouteReqDto>(RequestBodies.TestRoute1));
            

            //Assert
            var actionResult = Assert.IsType<ActionResult<RouteRespDto>>(response);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<RouteRespDto>(createdAtActionResult.Value);
            returnValue.Waypoints[0].Latitude.Should().BeApproximately(46.94753, 0.00001);
            returnValue.Waypoints[1].Latitude.Should().BeApproximately(46.92716, 0.00001);
            returnValue.Waypoints[2].Latitude.Should().BeApproximately(46.92040, 0.00001);
            returnValue.Waypoints[3].Latitude.Should().BeApproximately(46.90992, 0.00001);
            returnValue.Waypoints[4].Latitude.Should().BeApproximately(46.95574, 0.00001);
            returnValue.Waypoints[5].Latitude.Should().BeApproximately(46.94753, 0.00001);

        }

     

    }
}
