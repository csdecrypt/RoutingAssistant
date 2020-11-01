using Itinero;

namespace RoutingAssistant.DataLayer.Contracts
{
    public interface IOsmRouter
    {
        Router GetInstance();
    }
}
