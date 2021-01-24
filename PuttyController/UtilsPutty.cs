using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace PuttyController
{
    // todo: log errors somewhere findable
    public class UtilsPutty
    { 
 

        // run a command to completion
        //
        // putty returns 1 for errors, otherwise it should be the return value
        //
        // responsibility: centralize the putty process invokation to a single method
        static private int ExecuteToCompletion(string filename, string arguments, out string stdout, out string stderr)
        {
            return UtilsWindows.ExecuteToCompletion(filename, arguments, out stdout, out stderr );
        }
            
        static private Process StartPuttyShell(string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = PuttyConstants.GetPUTTYFilename();
            process.StartInfo.Arguments = arguments;
            process.Start();
            return process;
        }

       static public bool CopyFileToTarget(string user, string pw, string ip_addr, string src_filename, string dest_filename)
        {
            string arguments = String.Format("-l {0} -pw {1} {2} {3}:{4}",
                                                    user,
                                                    pw,
                                                    src_filename,
                                                    ip_addr,
                                                    dest_filename);
            string outstring, errstring;
            return (ExecuteToCompletion(PuttyConstants.GetPSCPFilename(), arguments, out outstring, out errstring) == 0);

        }

       static public bool CopyDirectoryToTarget(string user, string pw, string ip_addr, string src_filename, string dest_filename)
        {
            string arguments = String.Format("-r -l {0} -pw {1} {2} {3}:{4}",
                                                    user,
                                                    pw,
                                                    src_filename,
                                                    ip_addr,
                                                    dest_filename);
            string outstring, errstring;
            return (ExecuteToCompletion(PuttyConstants.GetPSCPFilename(), arguments, out outstring, out errstring) == 0);

        }

       static public bool CopyFileFromTarget(string user, string pw, string ip_addr, string src_filename, string dest_filename)
        {
            string arguments = String.Format("-l {0} -pw {1} {2}:{3} {4}",
                                                    user, // 0
                                                    pw, // 1
                                                    ip_addr, // 2
                                                    src_filename, // 3
                                                    dest_filename);
            string outstring, errstring;
            return (ExecuteToCompletion(PuttyConstants.GetPSCPFilename(), arguments, out outstring, out errstring) == 0);
        }

       static public bool CopyDirectoryFromTarget(string user, string pw, string ip_addr, string src_filename, string dest_filename)
        {
            string arguments = String.Format("-r -l {0} -pw {1} {2}:{3} {4}",
                                                    user, // 0
                                                    pw, // 1
                                                    ip_addr, // 2
                                                    src_filename, // 3
                                                    dest_filename);
            string outstring, errstring;
            return (ExecuteToCompletion(PuttyConstants.GetPSCPFilename(), arguments, out outstring, out errstring) == 0);
        }


       static public int ShellExecuteToCompletion(string user, string pw, string ip_addr, string remote_filename, 
                                                    out string stdout, out string stderr)
        {
            // Algorithm create a 'putty command script'
            // create a command file, stuff the command to execute in there, then execute it
            string local_temp_file = System.IO.Path.GetTempFileName();
            using (var file = new StreamWriter(local_temp_file))
            {
                file.Write("bash -e " + remote_filename + "\n");
            }

            string arguments = String.Format("-ssh {0}@{1} -pw {2} -m {3} -batch",
                                                    user,
                                                    ip_addr,
                                                    pw,
                                                    local_temp_file);

            int result = ExecuteToCompletion(PuttyConstants.GetPLINKFilename(), arguments, out stdout, out stderr);

            System.IO.File.Delete(local_temp_file);

            return result;
        }



       static public Process StartPuttyShell(string user, string pw, string ip_addr)
        {
            string arguments = String.Format("-ssh {0}@{1} -pw {2}", user, ip_addr, pw);
            return StartPuttyShell(arguments);
        }
    }

    public class PuttyShell 
    {
        private Process _process;
        public bool Start(string user, string pw, string ip_addr)
        {
            _process = UtilsPutty.StartPuttyShell(user, pw, ip_addr);
            return true;
        }
        public bool Kill()
        {
            _process.Kill();
            return true;
        }

        public bool Done { get { return false; } }
        public int ReturnCode { get { return 0; } }
    }

    public class PuttyCommand
    {
        public int RunScriptToCompletion(string user, string pw, string ip_addr, string script, 
                    out string stdout, out string stderr)
        {
            // 0. Remove any DOS line endings
            script = script.Replace("\r\n", "\n");

            // 1. Store script in a local file
            string local_temp_file = System.IO.Path.GetTempFileName();
            using (var file = new StreamWriter(local_temp_file))
            {
                file.Write(script);
            }

            // 2. copy this local file to a remote file
            string remote_filename = "/tmp/target_helper.sh"; // todo: replace this with something that wontt collide
            bool copy_result = UtilsPutty.CopyFileToTarget(user, pw, ip_addr, local_temp_file, remote_filename);

            int result;
            if (copy_result)
            {
                // 3. execute the remote file
                result = UtilsPutty.ShellExecuteToCompletion(user, pw, ip_addr, remote_filename, out stdout, out stderr);
            }
            else
            {
                result = -1; // seans putty rule: -1 is error from putty
                stdout = "";
                stderr = "";
            }
            return result;
        }
    }
}
