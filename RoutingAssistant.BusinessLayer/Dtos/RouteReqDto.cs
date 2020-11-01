using RoutingAssistant.Core.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoutingAssistant.BusinessLayer.Dtos
{
    public class RouteReqDto
    {
        [Required]
        [EnsureMinimumElements(2, ErrorMessage = "At least two Stops are required")]
        public List<StopReqDto> Stops { get; set; }
    }
}
