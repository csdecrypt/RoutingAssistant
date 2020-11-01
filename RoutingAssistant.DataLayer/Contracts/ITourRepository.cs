using RoutingAssistant.Core.Entities;
using System.Threading.Tasks;

namespace RoutingAssistant.DataLayer.Contracts
{
    public interface ITourRepository
    {
        Task SaveTour(TourEntity entity);
        Task<TourEntity> LoadTour(int tourId);
        Task<bool> CheckTourExists(int routeId);
        Task<bool> CheckWayPointExists(int routeId, int waypointIndex);
    }
}
