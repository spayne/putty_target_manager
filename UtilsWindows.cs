using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace TargetManager
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

        static public void CreateFolderIfItDoestExist(string directory)
        {
            Directory.CreateDirectory(directory);
        }

        static public void OpenExplorerWindow(string directory)
        {
            Process.Start("explorer", directory);
        }

        static public void SetEnvironmentVariable(string key, string value)
        {
            System.Environment.SetEnvironmentVariable(key, value);
        }

        // todo: rename this StartBatchFile
        static public void StartCommand(string command, string arguments)
        {
            ProcessStartInfo si = new ProcessStartInfo();
            si.FileName = command;
            si.Arguments = arguments;
            
            Process.Start(si);
        }
    }
}
