using RoutingAssistant.BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAssistant.BusinessLayer.Contracts
{
    public interface IArrivalTimeService
    {
        Task<IEnumerable<ArrivalTimeRespDto>> GetArrivalTimes(int routeId, DateTime startDate);
        Task<ArrivalTimeRespDto> GetArrivalTime(int routeId, int waypointId);
    }
}
