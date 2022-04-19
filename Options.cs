using CommandLine;

namespace PgnCli
{
    [Verb("glicko", false, HelpText = "Prints player glicko ratings for the supplied pgn file.")]
    public class GlickoOptions
    {
        [Value(0, MetaName = "input file", HelpText = "File to be processed.", Required = true)]
        string File {get; set;}
    }
}
