using Microsoft.AspNetCore.Http;
using System.Net;

namespace MilkMatrix.Api.Common.Utils
{
    public static class HttpExtensions
    {
        public static void GetUserBrowserDetails(this IHttpContextAccessor httpContextAccessor, out IPAddress? publicIpAddress, out IPAddress? privateIpAddress, out string? userAgent)
        {
            publicIpAddress = httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress;
            privateIpAddress = httpContextAccessor?.HttpContext?.Connection.LocalIpAddress;
            userAgent = httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"].ToString();
            GetIpAddress(ref publicIpAddress, ref privateIpAddress);
        }

        private static void GetIpAddress(ref IPAddress? publicIpAddress, ref IPAddress? privateIpAddress)
        {
            if (publicIpAddress?.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                publicIpAddress = Dns.GetHostEntry(publicIpAddress).AddressList
          .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            }
            if (privateIpAddress?.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                privateIpAddress = Dns.GetHostEntry(privateIpAddress).AddressList
          .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }
        }
    }
}
