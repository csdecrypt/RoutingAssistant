using System.Collections.Generic;

namespace RoutingAssistant.BusinessLayer.Dtos.geojson
{
    public class LineString : IGeometryDto
    {
        public string type => "LineString";
        public List<List<double>> coordinates { get; set; }
    }
}
