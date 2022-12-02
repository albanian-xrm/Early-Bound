using System.Diagnostics;

namespace Helpers
{
    internal class ProcessHelper
    {
        public static Process getProcess(string fileName)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.EnableRaisingEvents = true;
            process.StartInfo.FileName = fileName;
            return process;
        }
    }
}
