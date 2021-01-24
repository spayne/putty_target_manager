using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace PuttyController
{
    class UtilsWindows
    {      
        static public int ExecuteToCompletion(string filename, string arguments, out string stdout, out string stderr)
        {   
            int result;
            using (var process = new Process())
            {
                process.StartInfo.FileName = filename;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();

                result = process.ExitCode;
            }
            return result;
        }
    }
}
