using Newtonsoft.Json.Linq;

namespace PgnCli
{
    public static class Import
    {
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

        public static JArray PgnToJson(string path)
        {
            dynamic json = JArray.Parse(PythonRun($"{Variables.ProgramPath}\\src\\py\\pgntojson.py", new List<string>{path}));
            return json;
        }
    }
}
