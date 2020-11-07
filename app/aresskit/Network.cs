using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace aresskit
{
    public class Network
    {
        public static bool checkInternetConn(string server)
        {
            try
            {
                using (Ping pingSender = new Ping())
                {
                    PingReply reply = pingSender.Send(server);
                    return reply.Status == IPStatus.Success ? true : false;
                }
            } catch (PingException)
            { return false; }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ipList = default(string);
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    ipList += ip.ToString() + "\n";
            }
            return ipList;
            throw new Exception("Local IP Address Not Found!");
        }

        public static string GetPublicIPAddress()
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("user-agent", "curl");
                return client.DownloadString("http://ipinfo.io/");
            }
        }
    }
}
