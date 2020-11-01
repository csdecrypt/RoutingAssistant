namespace RoutingAssistant.Core.Configuration
{
    public class TravelServiceOptions
    {
        public const string SectionName = "TravelServiceOptions";

        public string RouterDBPath { get; set; }
        public string RouterDBFileName { get; set; }
    }
}
