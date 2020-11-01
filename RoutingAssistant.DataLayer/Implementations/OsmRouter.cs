using Itinero;
using Microsoft.Extensions.Options;
using RoutingAssistant.Core.Configuration;
using RoutingAssistant.DataLayer.Contracts;
using System.IO;

namespace RoutingAssistant.DataLayer
{
    public class OsmRouter : IOsmRouter
    {
        private readonly RouterDb routerDb;
        private readonly Router router;
        public OsmRouter(IOptions<TravelServiceOptions> options)
        {
            using (var stream = new FileInfo(Path.Combine(options.Value.RouterDBPath, options.Value.RouterDBFileName)).OpenRead())
            {
                routerDb = RouterDb.Deserialize(stream);
            }
            router = new Router(routerDb);
        }

        public Router GetInstance()
        {
            return router;
        }
    }
}
