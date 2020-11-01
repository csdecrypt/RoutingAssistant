using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoutingAssistant.BusinessLayer;
using RoutingAssistant.BusinessLayer.Contracts;
using RoutingAssistant.BusinessLayer.Dtos;

namespace RoutingAssistant.Controllers
{
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteConstructionService _routeConstructionService;
        private readonly IArrivalTimeService _arrivalTimeService;

        public RouteController(IRouteConstructionService routeConstructionService, IArrivalTimeService arrivalTimeService)
        {
            _routeConstructionService = routeConstructionService;
            _arrivalTimeService = arrivalTimeService;
        }

        [HttpPost]
        [Route("route")]
        [ProducesResponseType(201, Type = typeof(RouteRespDto))]
        public async Task<ActionResult<RouteRespDto>> CreateRoute([FromBody] RouteReqDto reqRoute)
        {
            var coordinates = reqRoute.Stops.Select(s => new Stop(s.Latitude, s.Longitude)).ToList();
            var tour = await _routeConstructionService.CreateTour(coordinates);
            return CreatedAtAction(nameof(GetRoute), new { routeId = tour.Id }, tour);
        }

        [HttpGet]
        [Route("route/{routeId}")]
        [Produces(typeof(RouteRespDto))]
        public async Task<ActionResult<RouteRespDto>> GetRoute([FromRoute] int routeId, [FromQuery] bool geoJson = true)
        {
            var exists = await _routeConstructionService.CheckTourExists(routeId);
            if (!exists)
            {
                return NotFound();
            }
            var tour = await _routeConstructionService.LoadTour(routeId, geoJson);
            return Ok(tour);
        }

        [HttpGet]
        [Route("route/{routeId}/arrivaltimes")]
        [Produces(typeof(IEnumerable<ArrivalTimeRespDto>))]
        public async Task<IActionResult> GetArrivalTimes([FromRoute] int routeId, [FromQuery] DateTime? startDate)
        {
            if (!startDate.HasValue) return BadRequest();
            var exists = await _routeConstructionService.CheckTourExists(routeId);
            if (!exists)
            {
                return NotFound();
            }
            var arrivalTimes = await _arrivalTimeService.GetArrivalTimes(routeId, startDate.Value);
            return Ok(arrivalTimes);

        }

        [HttpGet]
        [Route("route/{routeId}/arrivaltimes/{waypointId}")]
        [Produces(typeof(ArrivalTimeRespDto))]
        public async Task<IActionResult> GetArrivalTime([FromRoute] int routeId, [FromRoute] int waypointId)
        {
            var pointExists = await _routeConstructionService.CheckWayPointExists(routeId, waypointId+1);
            if (!pointExists)
            {
                return NotFound();
            }
            var arrivalTime = await _arrivalTimeService.GetArrivalTime(routeId, waypointId);
            return Ok(arrivalTime);
        }


    }
}
