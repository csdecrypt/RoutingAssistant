using RoutingAssistant.BusinessLayer.Dtos;
using RoutingAssistant.Core.Entities;
using System;
using System.Collections.Generic;

namespace RoutingAssistant.BusinessLayer.Contracts
{
    public interface IMappingService
    {
        TourEntity MapToEntity(Tour tour);
        RouteRespDto MapToDto(TourEntity tourEntity, bool includeGeoJson = true);
        Tour MapToClass(TourEntity tourEntity);
        Stop MapToClass(StopEntity stopEntity);
        IEnumerable<ArrivalTimeRespDto> MapToDto(Dictionary<Stop, DateTime> arrivalTimes);
    }
}
