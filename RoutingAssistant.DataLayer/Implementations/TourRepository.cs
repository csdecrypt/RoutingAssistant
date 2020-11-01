using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RoutingAssistant.Core.Entities;
using RoutingAssistant.DataLayer.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingAssistant.DataLayer.Implementations
{
    public class TourRepository : ITourRepository
    {
        private readonly RoutingAssistantDbContext _dbContext;
        public TourRepository(RoutingAssistantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckTourExists(int routeId)
        {
            return await _dbContext.Tours.AnyAsync(t => t.Id == routeId);
        }

        public async Task<bool> CheckWayPointExists(int routeId, int waypointIndex)
        {
            return await _dbContext.Tours.AnyAsync(t => t.Id == routeId && t.Stops.Any(s => s.WayPointIndex == waypointIndex));
        }

        public Task<TourEntity> LoadTour(int tourId)
        {
            return _dbContext.Tours.Include(t => t.Stops).FirstAsync(t => t.Id == tourId);
        }

        public async Task SaveTour(TourEntity entity)
        {
            _dbContext.Tours.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
