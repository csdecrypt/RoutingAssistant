using Itinero;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAssistant.Core
{
    public static class Helper
    {
        public static List<T> Reorder<T>(IEnumerable<T> sourceList, T newBaseElement) 
        {
            var sourceWithoutHead = sourceList.Skip(1).ToList();
            var indexOfBase = sourceWithoutHead.IndexOf(newBaseElement);
            var upperPartWithoutHead = sourceWithoutHead.Where((v, idx) => idx < indexOfBase);
            var lowerPartWithNewhead = sourceWithoutHead.Where((v, idx) => idx >= indexOfBase);

            return lowerPartWithNewhead.Concat(upperPartWithoutHead).Append(newBaseElement).ToList();
        }
    }
}
