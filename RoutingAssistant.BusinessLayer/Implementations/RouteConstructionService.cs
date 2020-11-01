using RoutingAssistant.BusinessLayer.Contracts;
using RoutingAssistant.BusinessLayer.Dtos;
using RoutingAssistant.Core;
using RoutingAssistant.Core.Entities;
using RoutingAssistant.DataLayer.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingAssistant.BusinessLayer.Implementations
{
    public class RouteConstructionService : IRouteConstructionService
    {
        private readonly ITravelService _travelService;
        private readonly IMappingService _mappingService;
        private readonly ITourRepository _tourRepository;

        public RouteConstructionService(ITravelService travelService, IMappingService mappingService, ITourRepository tourRepository)
        {
            _travelService = travelService;
            _mappingService = mappingService;
            _tourRepository = tourRepository;
        }

        public async Task<bool> CheckTourExists(int routeId)
        {
            return await _tourRepository.CheckTourExists(routeId);
        }

        public async Task<bool> CheckWayPointExists(int routeId, int waypointIndex)
        {
            return await _tourRepository.CheckWayPointExists(routeId, waypointIndex);
        }

        public async Task<RouteRespDto> CreateTour(List<Stop> stops)
        {
            var tour = ConstructTour(stops);
            var tourEntity = await PerisistTour(tour);
            return _mappingService.MapToDto(tourEntity);
        }

        public async Task<RouteRespDto> LoadTour(int routeId, bool geoJson)
        {
            var tourEntity = await _tourRepository.LoadTour(routeId);
            return _mappingService.MapToDto(tourEntity, geoJson);
        }

        private Tour ConstructTour(List<Stop> stops)
        {
            var tour = new Tour(stops);
            _travelService.NormalizeStops(tour.Stops);
            _travelService.CalculateDistanceMatrix(tour);

            var (stop0, stop1) = tour.GetFarthestPair();
            tour.TourStops.Add(stop0);
            tour.TourStops.Add(stop1);
            tour.TourStops.Add(stop0);


            while (tour.TourStops.Count < stops.Count() + 1)
            {
                var farthestStop = tour.FindFarthestStop();
                tour.InsertAtLeastExpensiveEdge(farthestStop);
            }

            if (tour.TourStops.First() != tour.Stops.First())
            {
                var desiredStart = tour.TourStops.Single(ts => ts == tour.Stops.First());
                tour.TourStops = Helper.Reorder(tour.TourStops, desiredStart);
            }

            return tour;
        }

        private async Task<TourEntity> PerisistTour(Tour tour)
        {
            var tourEntity = _mappingService.MapToEntity(tour);
            await _tourRepository.SaveTour(tourEntity);
            return tourEntity;
        }
    }
}
