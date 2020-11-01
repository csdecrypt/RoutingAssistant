using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoutingAssistant.BusinessLayer.Contracts;
using RoutingAssistant.BusinessLayer.Implementations;
using RoutingAssistant.DataLayer;
using RoutingAssistant.DataLayer.Contracts;
using RoutingAssistant.DataLayer.Implementations;

namespace RoutingAssistant.Root
{
    public static class CompositionRoot
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton<IOsmRouter, OsmRouter>();
            services.AddTransient<ITravelService, TravelService>();
            services.AddTransient<IRouteConstructionService, RouteConstructionService>();
            services.AddTransient<IMappingService, MappingService>();
            services.AddTransient<IArrivalTimeService, ArrivalTimeService>();


            services.AddDbContext<RoutingAssistantDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("RoutingAssistantDB")));
            services.AddTransient<ITourRepository, TourRepository>();

        }
    }
}
