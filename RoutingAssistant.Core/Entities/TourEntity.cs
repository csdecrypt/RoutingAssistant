using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoutingAssistant.Core.Entities
{
    public class TourEntity
    {
        [Key]
        public int Id { get; set; }
        public List<StopEntity> Stops { get; set; }
    }
}
