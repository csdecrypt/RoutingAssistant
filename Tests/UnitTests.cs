using FluentAssertions;
using RoutingAssistant.BusinessLayer;
using RoutingAssistant.Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Tour_Should
    {
        [Fact]
        public void Set_Id_Correctly()
        {
            //Arrange
            var coordinates = new List<Stop> { new Stop(43, 16), new Stop(42, 13) };

            //Act
            var tour = new Tour(coordinates);

            //Assert
            tour.Stops.Count.Should().Be(coordinates.Count);
            tour.Stops.Single(s => s.Latitude == 43).Id.Should().Be(0);
            tour.Stops.Single(s => s.Latitude == 42).Id.Should().Be(1);
        }

        [Fact]
        public void Find_Farthest_Stop()
        {
            //Arrange
            var stop1 = new Stop(43, 16);
            var stop2 = new Stop(42, 13);
            var stop3 = new Stop(41, 12);
            var coordinates = new List<Stop> { stop1, stop2, stop3};
            var tour = new Tour(coordinates);
            tour.TourStops = new List<Stop> { stop1 };
            tour.DistanceMatrix = new float[][] {
                 new float[] { 0, 1, 3 },
                 new float[] { 1, 0, 4 },
                 new float[] { 3, 4, 5,} 
            };

            //Act
            var farthestStop = tour.FindFarthestStop();

            //Assert
            farthestStop.Should().Be(stop3);
        }
    }

    public class Helper_Should
    {
        [Fact]
        public void Reorder_SimpleType()
        {
            //Arrange
            var initialList = new List<int> { 1, 2, 3, 4, 5, 1 };
            var newBaseElement = 3;

            //Act
            var result = Helper.Reorder(initialList, newBaseElement);

            //Assert
            result.Count.Should().Be(initialList.Count);
            result[0].Should().Be(newBaseElement);
            result[1].Should().Be(4);
            result[2].Should().Be(5);
            result[3].Should().Be(1);
            result[4].Should().Be(2);
            result[5].Should().Be(newBaseElement);
        }

        [Fact]
        public void Reorder_ComplexType()
        {
            //Arrange
            var initialList = new List<Stop>
            {
                new Stop(3,3),
                new Stop(2,2),
                new Stop(1,1),
                new Stop(3,3)

            };
            //Act
            var newBaseElement = initialList.ElementAt(2);
            var result = Helper.Reorder(initialList, newBaseElement);

            //Assert
            result.Count.Should().Be(initialList.Count);
            result[0].Latitude.Should().Be(newBaseElement.Latitude);
            result[1].Latitude.Should().Be(3);
            result[2].Latitude.Should().Be(2);
            result[0].Latitude.Should().Be(newBaseElement.Latitude);
        }
    }
}
