using CommandLine;

namespace PgnCli
{
    [Verb("glicko", false, HelpText = "Prints player glicko ratings for the supplied pgn file.")]
    public class GlickoOptions
    {
        [Value(0, MetaName = "input file", HelpText = "File to be processed.", Required = true)]
        public string File {get; set;}

        [Option('v', "verbose", HelpText = "Show more info about ratings.")]
        public bool Verbose {get; set;}

        [Option('d', "max-deviation", HelpText = "Max deviation of which to show ratings.")]
        public int MaxDeviation {get; set;}
    }
}
