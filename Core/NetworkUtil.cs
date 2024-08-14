using System.Net;
using System.Net.Sockets;

namespace DotnetRateLimitAPI.Core
{
    public class NetworkUtil
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return string.Join(",", host?.AddressList?.Where(ip => AddressFamily.InterNetwork.Equals(ip.AddressFamily)));
        }
    }
}
