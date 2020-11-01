using Itinero;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoutingAssistant.BusinessLayer
{
    public class Stop
    {
        public Stop(float latitude, float longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public Stop(double latitude, double longitude) : this((float)latitude, (float)longitude)
        {
        }

        public int Id { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        //Streetnormalized Coordinate
        public RouterPoint RouterPoint { get; set; }
    }
}
