namespace PgnCli
{
    public static class Verbs
    {
        public static void Glicko(GlickoOptions options)
        {
            var unsortedPgn = Import.PgnToJson(System.IO.Path.GetFullPath(options.File)); // Import pgn into JArray from path
            var pgn = new Newtonsoft.Json.Linq.JArray(unsortedPgn.OrderBy(obj => DateTime.ParseExact(obj["_tag_roster"]["Date"].ToObject<string>(), "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture))); // Sort pgn by game date
            Console.WriteLine(pgn);
        }
    }
}
