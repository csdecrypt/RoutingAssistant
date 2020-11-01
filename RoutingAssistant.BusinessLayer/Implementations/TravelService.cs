using Itinero;
using RoutingAssistant.BusinessLayer.Contracts;
using RoutingAssistant.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAssistant.BusinessLayer.Implementations
{
    public class TravelService : ITravelService
    {
        private readonly IOsmRouter _osmRouter;
        public TravelService(IOsmRouter osmRouter)
        {
            _osmRouter = osmRouter;
        }

        /// <summary>
        /// Try to find a coordinate located on a street, max distance to actual coordinate 1000m
        /// </summary>
        /// <param name="tour"></param>
        public void NormalizeStops(List<Stop> stops)
        {
            foreach (var stop in stops)
            {
                var conntectedPoint = _osmRouter.GetInstance().ResolveConnected(Itinero.Osm.Vehicles.Vehicle.Car.Shortest(), stop.Latitude, stop.Longitude, maxSearchDistance: 1000);
                stop.RouterPoint = conntectedPoint;
            }
        }

        public void CalculateDistanceMatrix(Tour tour)
        {
            if (!tour.StopsNormalized) throw new Exception("Expecting all Stops to be normalized first");
            var invalids = new HashSet<int>(); // will hold the locations that cannot be calculated.
            var result = _osmRouter.GetInstance().CalculateWeight(Itinero.Osm.Vehicles.Vehicle.Car.Shortest(), tour.Stops.Select(c => c.RouterPoint).ToArray(), invalids);
            if (invalids.Any()) throw new Exception("Invalid waypoint in tour");

            tour.DistanceMatrix = result;
        }

        public void CalculateTimeMatrix(Tour tour)
        {
            if (!tour.TourStopsNormalized) throw new Exception("Expecting all Stops to be normalized first");
            var invalids = new HashSet<int>(); // will hold the locations that cannot be calculated.
            var result = _osmRouter.GetInstance().CalculateWeight(Itinero.Osm.Vehicles.Vehicle.Car.Fastest(), tour.TourStops.Select(c => c.RouterPoint).ToArray(), invalids);
            if (invalids.Any()) throw new Exception("Invalid waypoint in tour");

            tour.TimeMatrix = result;
        }

        public float GetTravelTime(Stop startingPoint, Stop endPoint)
        {
            var miniTour = new List<Stop> { startingPoint, endPoint };
            NormalizeStops(miniTour);
            var route =  _osmRouter.GetInstance().Calculate(Itinero.Osm.Vehicles.Vehicle.Car.Fastest(), startingPoint.RouterPoint, endPoint.RouterPoint);
            return route.TotalTime;
        }
    }
}
