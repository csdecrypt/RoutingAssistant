using CommandLine;

namespace RouterDbCreator
{
    class Program
    {
        public static void Main(string[] args) => Parser.Default.ParseArguments<Options>(args).WithParsed(opts => DBCreator.Build(opts));

        
    }
}
