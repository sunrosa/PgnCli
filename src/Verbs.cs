namespace PgnCli
{
    public static class Verbs
    {
        public static void Glicko(GlickoOptions options)
        {
            Console.WriteLine(Import.PgnToJson($"{Variables.ProgramPath}\\env\\lichess.pgn"));
        }
    }
}
