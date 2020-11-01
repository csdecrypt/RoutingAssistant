using RoutingAssistant.BusinessLayer.Contracts;
using RoutingAssistant.BusinessLayer.Dtos;
using RoutingAssistant.BusinessLayer.Dtos.geojson;
using RoutingAssistant.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAssistant.BusinessLayer.Implementations
{
    public class MappingService : IMappingService
    {
        #region Class2Entity
        public TourEntity MapToEntity(Tour tour)
        {
            var entity = new TourEntity
            {
                Id = tour.Id,
                Stops = new List<StopEntity>()
            };

            for (int i = 0; i < tour.TourStops.Count; i++)
            {
                Stop stop = tour.TourStops[i];
                var stopEntity = new StopEntity
                {
                    Latitude = stop.Latitude,
                    Longitude = stop.Longitude,
                    Tour = entity,
                    WayPointIndex = i
                };
                entity.Stops.Add(stopEntity);
            }
            return entity;
        }
        #endregion

        #region Class2Dto
        public IEnumerable<ArrivalTimeRespDto> MapToDto(Dictionary<Stop, DateTime> arrivalTimes)
        {
            var dto = new List<ArrivalTimeRespDto>();
            foreach (var stop in arrivalTimes)
            {
                dto.Add(new ArrivalTimeRespDto
                {
                    WaypointId = stop.Key.Id,
                    ArrivalTime = stop.Value
                });
            }
            return dto;
        }
        #endregion

        #region Entity2Class
        public Tour MapToClass(TourEntity tourEntity)
        {
            return new Tour(tourEntity.Stops.Skip(1).Select(s => new Stop(s.Latitude, s.Longitude)).ToList())
            {
                Id = tourEntity.Id,
                TourStops = tourEntity.Stops
                .OrderBy(s => s.WayPointIndex)
                .Select(s => MapToClass(s)).ToList()
            };
        }

        public Stop MapToClass(StopEntity stopEntity)
        {
            return new Stop(stopEntity.Latitude, stopEntity.Longitude) { Id = stopEntity.WayPointIndex };
        }
        #endregion

        #region Entity2Dto
        public RouteRespDto MapToDto(TourEntity tourEntity, bool includeGeoJson = true)
        {
            var dto = new RouteRespDto
            {
                Id = tourEntity.Id,
                Waypoints = tourEntity.Stops.OrderBy(s => s.WayPointIndex).Select(s => MapToDto(s)).ToList(),
                GeoJson = includeGeoJson ? new FeatureCollectionDto
                {
                    features = new List<FeatureDto>
                    {
                        new FeatureDto{
                            geometry = new LineString
                            {
                                coordinates = tourEntity.Stops
                                .OrderBy(s => s.WayPointIndex)
                                .Select(s => new List<double> { s.Longitude, s.Latitude })
                                .ToList()
                            }
                        }
                    }
                } : null
            };

            return dto;
        }

        private WayPointDto MapToDto(StopEntity stopEntity)
        {
            return new WayPointDto
            {
                Id = stopEntity.WayPointIndex,
                Latitude = stopEntity.Latitude,
                Longtude = stopEntity.Longitude,
            };
        }


        #endregion

        #region Dto2Entity
        #endregion





    }
}
