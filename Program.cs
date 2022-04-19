using System;
using CommandLine;

namespace PgnCli
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Verbose output")]
        public bool Verbose {get; set;}
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                if (o.Verbose)
                {
                    Console.WriteLine("Verbose!!!");
                }
            });
        }
    }
}
