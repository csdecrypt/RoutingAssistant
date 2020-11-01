using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAssistant.BusinessLayer
{
    public class Tour
    {
        public int Id { get; set; }
        /// <summary>
        /// DistanceMatrix of Stops
        /// </summary>
        public float[][] DistanceMatrix { get; set; }

        /// <summary>
        /// TimeMatrix of TourStops
        /// </summary>
        public float[][] TimeMatrix { get; set; }

        /// <summary>
        /// Raw list of Stops, in input order
        /// </summary>
        public List<Stop> Stops { get; set; }

        /// <summary>
        /// Ordered List of Stops
        /// </summary>
        public List<Stop> TourStops { get; set; }

        public bool StopsNormalized => !Stops.Any(s => s.RouterPoint == null);
        public bool TourStopsNormalized => !TourStops.Any(s => s.RouterPoint == null);


        public bool DistanceMatrixReady => DistanceMatrix != null;
        public bool TimeMatrixReady => TimeMatrix != null;


        public Tour(List<Stop> coordinates)
        {
            Stops = new List<Stop>();
            TourStops = new List<Stop>();
            for (int i = 0; i < coordinates.Count; i++)
            {
                var stop = coordinates.ElementAt(i);
                stop.Id = i;
                Stops.Add(stop);
            }
        }

        /// <summary>
        /// Gets the two points out of the Stops collection
        /// </summary>
        /// <returns></returns>
        public (Stop stop0, Stop stop1) GetFarthestPair()
        {
            if (!DistanceMatrixReady) throw new Exception("DistanceMatrix needs to be calculated first");

            float currentMaxDistance = float.MinValue;
            Stop closestCandidateLeft = null;
            Stop closestCandidateRight = null;


            foreach (var verticalStop in Stops)
            {
                foreach (var horizontalStop in Stops)
                {
                    //Skip self distnace
                    if (verticalStop.Id == horizontalStop.Id) continue;
                    var distance = DistanceMatrix[verticalStop.Id][horizontalStop.Id];
                    if (distance > currentMaxDistance)
                    {
                        currentMaxDistance = distance;
                        closestCandidateLeft = verticalStop;
                        closestCandidateRight = horizontalStop;
                    }
                }
            }

            if (closestCandidateLeft == null || closestCandidateRight == null) throw new Exception("Cannot find farthest pair");
            return (closestCandidateLeft, closestCandidateRight);
        }

        /// <summary>
        /// Get the stop of Stop collection that is farthest from any Stop in TourStops
        /// </summary>
        /// <returns></returns>
        public Stop FindFarthestStop()
        {
            float currentMaxDistance = float.MinValue;
            Stop farthestCandidate = null;

            foreach (var pointInTour in TourStops)
            {
                foreach (var stop in Stops)
                {
                    //Exlude visited stops as candidates
                    if (TourStops.Any(ts => ts.Id == stop.Id)) continue;
                    var distance = DistanceMatrix[pointInTour.Id][stop.Id];
                    if (distance > currentMaxDistance)
                    {
                        currentMaxDistance = distance;
                        farthestCandidate = stop;
                    }
                }
            }

            if (farthestCandidate == null) throw new Exception("Cannot find farthest pair");
            return (farthestCandidate);
        }

        /// <summary>
        /// Insert Stop at least expensive edge of TourStops
        /// </summary>
        /// <param name="stopToAdd"></param>
        public void InsertAtLeastExpensiveEdge(Stop stopToAdd)
        {
            float currentMinInsertionCost = float.MaxValue;
            int insertionIndexCandidate = -1;

            //Iterate through every pair of the tour eg 0+1,1+2..
            for (int i = 0; i < TourStops.Count - 1; i++)
            {
                var leftCandidate = TourStops.ElementAt(i);
                var rightCandidate = TourStops.ElementAt(i + 1);

                var insertionCost = MinFunction(leftCandidate.Id, stopToAdd.Id, rightCandidate.Id);
                if (insertionCost < currentMinInsertionCost)
                {
                    currentMinInsertionCost = insertionCost;
                    insertionIndexCandidate = i + 1;
                }
            }

            TourStops.Insert(insertionIndexCandidate, stopToAdd);
        }

        public Dictionary<Stop, float> GetTimeTable()
        {
            if (!TimeMatrixReady) throw new Exception("DistanceMatrix needs to be calculated first");
            var timeTable = new Dictionary<Stop, float>();
            for (int i = 0; i < TourStops.Count-1; i++)
            {
                var stop = TourStops.ElementAt(i+1);
                var travelTime = TimeMatrix[i][i + 1];
                timeTable.Add(stop, travelTime);

            }
            return timeTable;
        }

        /// <summary>
        /// Function to calculate cost of adding a stop to TourStops
        /// </summary>
        /// <param name="leftCandidateId"></param>
        /// <param name="newStopId"></param>
        /// <param name="rightCandidateId"></param>
        /// <returns></returns>
        private float MinFunction(int leftCandidateId, int newStopId, int rightCandidateId)
        {
            return DistanceMatrix[leftCandidateId][newStopId] + DistanceMatrix[newStopId][rightCandidateId] - DistanceMatrix[leftCandidateId][rightCandidateId];
        }
    }
}