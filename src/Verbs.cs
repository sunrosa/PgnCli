namespace PgnCli
{
    public static class Verbs
    {
        public static void Glicko(GlickoOptions options)
        {
            Console.WriteLine(Import.PgnToJson(System.IO.Path.GetFullPath(options.File)));
        }
    }
}
