using System.ComponentModel.DataAnnotations;

namespace RoutingAssistant.Core.Entities
{
    public class StopEntity
    {
        [Key]
        public int Id { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int WayPointIndex { get; set; }

        public int TourId { get; set; }
        public TourEntity Tour { get; set; }
    }
}
