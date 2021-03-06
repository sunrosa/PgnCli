namespace PgnCli
{
    public static class Verbs
    {
        /// <summary>
        /// Create a DateTime object based on the date format written in PGN files.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static DateTime ParseDateTime(string date)
        {
            return DateTime.ParseExact(date, "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Create a dictionary of playernames and their full rating objects from a pgn JArray and the Glicko verb's command line options.
        /// </summary>
        /// <param name="pgn"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static Dictionary<string, Glicko2.Rating> GlickoRate(Newtonsoft.Json.Linq.JArray pgn, GlickoOptions options)
        {
            var sortedPgn = new Newtonsoft.Json.Linq.JArray(pgn.OrderBy(obj => ParseDateTime(obj["_tag_roster"]["Date"].ToObject<string>()))); // Sort pgn by game date

            var calculator = new Glicko2.RatingCalculator();
            var players = new Dictionary<string, Glicko2.Rating>();

            var results = new Glicko2.RatingPeriodResults();

            var currentDay = ParseDateTime(sortedPgn[0]["_tag_roster"]["Date"].ToObject<string>());

            foreach (var game in sortedPgn)
            {
                if (currentDay.Date != ParseDateTime(game["_tag_roster"]["Date"].ToObject<string>()).Date)
                {
                    // If the current date is different from the previous date, push the latest rating period, and create a new one.
                    calculator.UpdateRatings(results);
                    results = new Glicko2.RatingPeriodResults();
                    currentDay = ParseDateTime(game["_tag_roster"]["Date"].ToObject<string>());
                }

                var whiteName = game["_tag_roster"]["White"].ToObject<string>();
                var blackName = game["_tag_roster"]["Black"].ToObject<string>();

                var white = new Glicko2.Rating(calculator);
                var black = new Glicko2.Rating(calculator);

                if (players.ContainsKey(whiteName))
                    white = players[whiteName];
                else if (options.UseExistingRating && game["_others"]["WhiteElo"] != null)
                    white = new Glicko2.Rating(calculator, game["_others"]["WhiteElo"].ToObject<double>(), calculator.GetDefaultRatingDeviation(), calculator.GetDefaultVolatility());

                if (players.ContainsKey(blackName))
                    black = players[blackName];
                else if (options.UseExistingRating && game["_others"]["BlackElo"] != null)
                    black = new Glicko2.Rating(calculator, game["_others"]["BlackElo"].ToObject<double>(), calculator.GetDefaultRatingDeviation(), calculator.GetDefaultVolatility());

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

            calculator.UpdateRatings(results);

            return players;
        }

        /// <summary>
        /// UI interface for Glicko ratings.
        /// </summary>
        /// <param name="options"></param>
        public static void Glicko(GlickoOptions options)
        {
            var unsortedPgn = Import.PgnHeadersToJson(System.IO.Path.GetFullPath(options.File)); // Import pgn into JArray from path

            var ratings = GlickoRate(unsortedPgn, options).ToList().OrderByDescending(obj => obj.Value.GetRating());

            var outStr = new List<string>();

            if (!options.Verbose)
            {
                foreach (var rating in ratings)
                {
                    if (options.MaxDeviation != 0 && rating.Value.GetRatingDeviation() > options.MaxDeviation) continue; // Maximum rating deviation to display a rating
                    if (options.PlayerName != null && rating.Key != options.PlayerName) continue;

                    outStr.Add($"{(Math.Round(rating.Value.GetRating(), 2)).ToString("0.00")}{(rating.Value.GetRatingDeviation() > Variables.QuestionMarkMinimumDeviation ? "?" : " ")}: {rating.Key}");
                }
            }
            else
            {
                foreach (var rating in ratings)
                {
                    if (options.MaxDeviation != 0 && rating.Value.GetRatingDeviation() > options.MaxDeviation) continue; // Maximum rating deviation to display a rating
                    if (options.PlayerName != null && rating.Key != options.PlayerName) continue;

                    outStr.Add($"{(Math.Round(rating.Value.GetRating(), 2)).ToString("0.00")}{(rating.Value.GetRatingDeviation() > Variables.QuestionMarkMinimumDeviation ? "?" : " ")}: {rating.Key} ({rating.Value.GetRatingDeviation()} RD, {rating.Value.GetVolatility()} vol)");
                }
            }

            Console.WriteLine(string.Join('\n', outStr));
        }
    }
}
