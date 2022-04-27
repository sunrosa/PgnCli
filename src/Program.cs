using System;
using CommandLine;

namespace PgnCli
{
    public class Program
    {
        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<GlickoOptions, object>(args).WithParsed<GlickoOptions>(o => Verbs.Glicko(o)); // object is necessary because we only have one verb
        }
    }
}
