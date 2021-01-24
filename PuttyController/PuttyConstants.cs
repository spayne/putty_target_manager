using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuttyController
{
    static class PuttyConstants
    {
            static string PUTTY_ROOT = @"e:\PuttyBinaries";
            static public string GetPUTTYFilename()
            {
                return PUTTY_ROOT + "/putty.exe";
            }
            static public string GetPSCPFilename()
            {
                return PUTTY_ROOT + "/pscp.exe";
            }
            static public string GetPLINKFilename()
            {
                return PUTTY_ROOT + "/plink.exe";
            }
            static public string GetTargetImageFilename()
            {
                return @"e:\target_images\target_images99.tar.xz";
            }
            static public string GetUser()
            {
                return "ubuntu";
            }
            static public string GetPassword()
            {
                return "steve8878";
            }
            static public string GetIP()
            {
                return "10.46.194.154";
            }
    }
}
