
using System.Collections.Generic;

namespace RoutingAssistant.BusinessLayer.Contracts
{
    public interface ITravelService
    {
        /// <summary>
        /// Ensure Coordinate is located on a street
        /// </summary>
        void NormalizeStops(List<Stop> stops);
        /// <summary>
        /// Calculate Distance from every point to every point in Stops
        /// </summary>
        void CalculateDistanceMatrix(Tour tour);
        /// <summary>
        /// Calculate Distance from every point to every point in TourStops
        /// </summary>
        void CalculateTimeMatrix(Tour tour);
        /// <summary>
        /// Get Traveltime for two targets
        /// </summary>
        float GetTravelTime(Stop startingPoint, Stop endPoint);
    }
}
