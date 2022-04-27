using CommandLine;

namespace PgnCli
{
    /// <summary>
    /// Options for Glicko, which Prints player glicko ratings for the supplied pgn file.
    /// </summary>
    [Verb("glicko", false, HelpText = "Prints player glicko ratings for the supplied pgn file.")]
    public class GlickoOptions
    {
        /// <summary>
        /// The file to be processed.
        /// </summary>
        /// <value></value>
        [Value(0, MetaName = "input file", HelpText = "File to be processed.", Required = true)]
        public string File {get; set;}

        /// <summary>
        /// Whether or not to include more info about ratings.
        /// </summary>
        /// <value></value>
        [Option('v', "verbose", HelpText = "Show more info about ratings.")]
        public bool Verbose {get; set;}

        /// <summary>
        /// The max deviation of which to show ratings.
        /// </summary>
        /// <value></value>
        [Option('d', "max-deviation", HelpText = "Max deviation of which to show ratings.")]
        public int MaxDeviation {get; set;}

        /// <summary>
        /// The player name to search for.
        /// </summary>
        /// <value></value>
        [Option('p', "player-name", HelpText = "Player name to search for.")]
        public string PlayerName {get; set;}

        /// <summary>
        /// Whether or not to use existing rating numbers contained in pgn files for newly tracked ratings.
        /// </summary>
        /// <value></value>
        [Option('e', "existing-rating", HelpText = "Uses existing rating numbers contained in pgn files for newly tracked ratings.")]
        public bool UseExistingRating {get; set;}
    }
}
