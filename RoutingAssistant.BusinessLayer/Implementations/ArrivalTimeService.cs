using RoutingAssistant.BusinessLayer.Contracts;
using RoutingAssistant.BusinessLayer.Dtos;
using RoutingAssistant.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAssistant.BusinessLayer.Implementations
{
    public class ArrivalTimeService : IArrivalTimeService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMappingService _mappingService;
        private readonly ITravelService _travelService;

        public ArrivalTimeService(ITourRepository tourRepository, IMappingService mappingService, ITravelService travelService)
        {
            _tourRepository = tourRepository;
            _mappingService = mappingService;
            _travelService = travelService;
        }

        public async Task<ArrivalTimeRespDto> GetArrivalTime(int routeId, int waypointId)
        {
            var tourEntity = await _tourRepository.LoadTour(routeId);
            var stops = tourEntity.Stops
                                .OrderBy(s => s.WayPointIndex);
            var currentStopEntity = stops.Where(s => s.WayPointIndex == waypointId).First();
            var nextStopEntity = stops.Where(s => s.WayPointIndex > waypointId).First();
            var currentStop =  _mappingService.MapToClass(currentStopEntity);
            var nextStop = _mappingService.MapToClass(nextStopEntity);

            var travelTime = _travelService.GetTravelTime(currentStop, nextStop);
            return new ArrivalTimeRespDto
            {
                ArrivalTime = DateTime.Now.AddSeconds(travelTime),
                WaypointId = nextStopEntity.WayPointIndex
            };

        }

        public async Task<IEnumerable<ArrivalTimeRespDto>> GetArrivalTimes(int routeId, DateTime startDate)
        {
            var tourEntity = await _tourRepository.LoadTour(routeId);
            var tour = _mappingService.MapToClass(tourEntity);
            _travelService.NormalizeStops(tour.TourStops);
            _travelService.CalculateTimeMatrix(tour);
            var timeTable = tour.GetTimeTable();

            var arrivalTimes = new Dictionary<Stop, DateTime>();
            float currentTotalTravelTime = 0;
            foreach (var stop in timeTable)
            {
                currentTotalTravelTime += stop.Value;
                arrivalTimes.Add(stop.Key, startDate.AddSeconds(currentTotalTravelTime));
            }

            return _mappingService.MapToDto(arrivalTimes);
        }
    }
}
