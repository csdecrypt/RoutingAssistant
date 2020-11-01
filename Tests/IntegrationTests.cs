using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoutingAssistant;
using RoutingAssistant.BusinessLayer.Dtos;
using RoutingAssistant.DataLayer.Implementations;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class IntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public IntegrationTests()
        {
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("testappsettings.json");
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureServices(services =>
            {
                services.AddDbContext<RoutingAssistantDbContext>(options => options.UseInMemoryDatabase("Test"));
            }).UseConfiguration(configurationBuilder.Build()));
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task Refuse_Invalid_Coordinates()
        {
            //Arrange
            var request = new RouteReqDto()
            {
                Stops = new List<StopReqDto>
                {
                    new StopReqDto
                    {
                        Latitude = 100,
                        Longitude = 200
                    },
                    new StopReqDto
                    {
                        Latitude = 200,
                        Longitude = 400
                    }
                }
            };

            //Act
            var content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Add(Constants.ApiKey, "123123");
            var response = await _client.PostAsync("route", content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Refuse_Tour_With_OneStop()
        {
            //Arrange
            var request = new RouteReqDto()
            {
                Stops = new List<StopReqDto>
                {
                    new StopReqDto
                    {
                        Latitude = 47,
                        Longitude = 9
                    }
                }
            };

            //Act
            var content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Add(Constants.ApiKey, "123123");
            var response = await _client.PostAsync("route", content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
