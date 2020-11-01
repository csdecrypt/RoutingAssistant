using System;

namespace RoutingAssistant.BusinessLayer.Dtos.geojson
{
    public class FeatureDto
    {
        public string type => "Feature";
        public Object properties { get; set; }
        public IGeometryDto geometry { get; set; }
    }
}
