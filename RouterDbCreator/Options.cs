using CommandLine;

namespace RouterDbCreator
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Filepath to osm.pbf file")]
        public string OsmFilePath { get; set; }

        [Option('o', "out", Required = true, HelpText = "Directory to place router db")]
        public string Outputdirectory { get; set; }
    }
}
