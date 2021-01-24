using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuttyController
{
    class Program
    {

        static private bool UnzipImageOnTarget()
        {
            string target_filename = PuttyConstants.GetTargetImageFilename();
            UtilsPutty.CopyFileToTarget(PuttyConstants.GetUser(), PuttyConstants.GetPassword(), PuttyConstants.GetIP(),
                                            target_filename, "/tmp/image_to_unzip.zip");

            string unzip_script = String.Join(
                "\n",
                "#!/bin/sh",
                "cd /tmp",
                "tar xvf image_to_unzip.zip",
                "exit 123");

            string stdout, stderr;
            PuttyCommand command = new PuttyCommand();
            int return_code = command.RunScriptToCompletion(PuttyConstants.GetUser(), PuttyConstants.GetPassword(), PuttyConstants.GetIP(),
                                            unzip_script, out stdout, out stderr);

            return (return_code == 123);
        }

        static private bool RunInstallScript()
        {
            string run_install_script = String.Join(
                "\n",
                "#!/bin/sh",
                "cd /tmp",
                "source ./install_debs_helper.sh " + PuttyConstants.GetPassword(),
                "exit 123");

            string stdout, stderr;
            PuttyCommand command = new PuttyCommand();
            int return_code = command.RunScriptToCompletion(PuttyConstants.GetUser(), PuttyConstants.GetPassword(), PuttyConstants.GetIP(),
                                            run_install_script, out stdout, out stderr);

            return (return_code == 123);
        }

        static public void InstallTargetImage()
        {
            UnzipImageOnTarget();
            RunInstallScript();           
        }

        static void Main(string[] args)
        {
            InstallTargetImage();
        }
    }
}
