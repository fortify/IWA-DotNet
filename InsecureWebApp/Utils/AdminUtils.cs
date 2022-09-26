using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MicroFocus.InsecureWebApp.Utils
{
    public class AdminUtils
    {

        const String DEFAULT_ADMIN_PASSWORD = "ABCDEFG12345";

        public static int StartDbBackup(String profile)
        {
            int backupId = 0;

            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + "/K dir c:\\util\\backup.bat" + "-profile" + profile);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            // wrap IDisposable into using (in order to release hProcess) 
            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                // Add this: wait until process does its work
                process.WaitForExit();

                // and only then read the result
                string result = process.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }

            // call backup tool API
            backupId = GetBackupId();
            return backupId;
        }

        public static int GetBackupId()
        {
            return GenId();
        }

        private static int GenId()
        {
            Random r = new Random();
            int genRand = r.Next(100, 1000);
            return genRand;
        }
    }
}
