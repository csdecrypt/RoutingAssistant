using RoutingAssistant.BusinessLayer.Dtos.geojson;
using System.Collections.Generic;

namespace RoutingAssistant.BusinessLayer.Dtos
{
    public class RouteRespDto
    {
        public int Id { get; set; }
        public List<WayPointDto> Waypoints { get; set; }
        public FeatureCollectionDto GeoJson { get; set; }
    }
}
