using RoutingAssistant.BusinessLayer.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoutingAssistant.BusinessLayer.Contracts
{
    public interface IRouteConstructionService
    {
        Task<RouteRespDto> CreateTour(List<Stop> stops);
        Task<bool> CheckTourExists(int routeId);
        Task<bool> CheckWayPointExists(int routeId, int waypointIndex);
        Task<RouteRespDto> LoadTour(int routeId, bool geoJson);
    }
}
