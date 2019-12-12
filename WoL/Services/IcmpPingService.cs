using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using static WoL.Services.IPingService;

namespace WoL.Services
{
    public class IcmpPingService : DnsPingServiceBase
    {
        public IcmpPingService(IAddressLookupService addressLookupService) : base(addressLookupService)
        {
        }

        public override async Task<bool> IsReachable(IPAddress ip, TimeSpan timeout)
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(ip, (int)timeout.TotalMilliseconds).ConfigureAwait(false);
            return reply.Status == IPStatus.Success;
        }
    }
}
