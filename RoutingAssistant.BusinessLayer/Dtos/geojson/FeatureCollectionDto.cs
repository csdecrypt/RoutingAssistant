using System.Collections.Generic;

namespace RoutingAssistant.BusinessLayer.Dtos.geojson
{
    public class FeatureCollectionDto
    {
        public string type => "FeatureCollection";
        public List<FeatureDto> features { get; set; }
    }
}
