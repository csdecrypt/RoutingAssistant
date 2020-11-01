using System.Collections.Generic;

namespace RoutingAssistant.BusinessLayer.Dtos.geojson
{
    public interface IGeometryDto
    {
        string type { get; }
        List<List<double>> coordinates { get; set; }
    }
}
