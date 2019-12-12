using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using WoL.Extensions;

namespace WoL.Services
{
    public class RdpPortPingService : DnsPingServiceBase
    {
        public RdpPortPingService(IAddressLookupService addressLookupService) : base(addressLookupService)
        {
        }

        private static async Task<bool> IsPortOpen(IPAddress ip, int port, TimeSpan timeout)
        {
            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(ip, port).TimeoutAfter(timeout).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override Task<bool> IsReachable(IPAddress ip, TimeSpan timeout)
        {
            return IsPortOpen(ip, 3389, timeout);
        }
    }
}
