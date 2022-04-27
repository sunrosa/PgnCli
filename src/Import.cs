using Newtonsoft.Json.Linq;

namespace PgnCli
{
    public static class Import
    {
        /// <summary>
        /// [PYTHON BACKEND] Run something in the local machine's python interpreter.
        /// </summary>
        /// <param name="cmd">Full path to the command to be run.</param>
        /// <param name="args">List of arguments to be passed.</param>
        /// <returns></returns>
        private static string PythonRun(string cmd, List<string> args = null)
        {
            var start = new System.Diagnostics.ProcessStartInfo();
            start.FileName = Variables.PythonPath;
            if (args != null)
                start.Arguments = $"{cmd} {String.Join(' ', args)}";
            else
                start.Arguments = cmd;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using(var process = System.Diagnostics.Process.Start(start))
            {
                using(var reader = process.StandardOutput)
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// [PYTHON BACKEND] Convert a pgn file to a JArray.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static JArray PgnHeadersToJson(string path)
        {
            dynamic json = JArray.Parse(PythonRun($"{Variables.ProgramPath}\\src\\py\\PgnHeadersToJson.py", new List<string>{path}));
            return json;
        }
    }
}
