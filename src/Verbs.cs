namespace PgnCli
{
    public static class Verbs
    {
        private static DateTime ParseDateTime(string date)
        {
            return DateTime.ParseExact(date, "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        private static Dictionary<string, Glicko2.Rating> GlickoRate(Newtonsoft.Json.Linq.JArray pgn)
        {
            var sortedPgn = new Newtonsoft.Json.Linq.JArray(pgn.OrderBy(obj => ParseDateTime(obj["_tag_roster"]["Date"].ToObject<string>()))); // Sort pgn by game date

            var calculator = new Glicko2.RatingCalculator();
            var players = new Dictionary<string, Glicko2.Rating>();

            var results = new Glicko2.RatingPeriodResults();

            var currentDay = new DateTime();

            foreach (var game in sortedPgn)
            {
                if (currentDay.Date == new DateTime().Date)
                {
                    // Executes during start of loop
                    currentDay = ParseDateTime(game["_tag_roster"]["Date"].ToObject<string>());
                }
                else if (currentDay.Date != ParseDateTime(game["_tag_roster"]["Date"].ToObject<string>()).Date)
                {
                    // If the current date is different from the previous date, push the latest rating period, and create a new one.
                    calculator.UpdateRatings(results);
                    results = new Glicko2.RatingPeriodResults();
                }

                var whiteName = game["_tag_roster"]["White"].ToObject<string>();
                var blackName = game["_tag_roster"]["Black"].ToObject<string>();

                var white = new Glicko2.Rating(calculator);
                var black = new Glicko2.Rating(calculator);

                if (players.ContainsKey(whiteName))
                    white = players[whiteName];

                if (players.ContainsKey(blackName))
                    black = players[blackName];

                if (game["_tag_roster"]["Result"].ToObject<string>() == "1-0")
                {
                    results.AddResult(white, black);
                }
                else if (game["_tag_roster"]["Result"].ToObject<string>() == "0-1")
                {
                    results.AddResult(black, white);
                }
                else if (game["_tag_roster"]["Result"].ToObject<string>() == "1/2-1/2")
                {
                    results.AddDraw(white, black);
                }

                players[whiteName] = white;
                players[blackName] = black;
            }

            return players;
        }

        public static void Glicko(GlickoOptions options)
        {
            var unsortedPgn = Import.PgnToJson(System.IO.Path.GetFullPath(options.File)); // Import pgn into JArray from path

            var ratings = GlickoRate(unsortedPgn).ToList().OrderByDescending(obj => obj.Value.GetRating());

            var outStr = "";

            if (!options.Verbose)
            {
                foreach (var rating in ratings)
                {
                    outStr += $"{(Math.Round(rating.Value.GetRating(), 2)).ToString("0.00")}{(rating.Value.GetRatingDeviation() > 200 ? "?" : "")}: {rating.Key}\n";
                }
            }
            else
            {
                foreach (var rating in ratings)
                {
                    outStr += $"{(Math.Round(rating.Value.GetRating(), 2)).ToString("0.00")}{(rating.Value.GetRatingDeviation() > 200 ? "?" : "")}: {rating.Key} ({rating.Value.GetRatingDeviation()} RD, {rating.Value.GetVolatility()} vol)\n";
                }
            }

            Console.WriteLine(outStr);
        }
    }
}
