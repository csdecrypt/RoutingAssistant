using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingAssistant.BusinessLayer.Dtos
{
    public class ArrivalTimeRespDto
    {
        public int WaypointId { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
