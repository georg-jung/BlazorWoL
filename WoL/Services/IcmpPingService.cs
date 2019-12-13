using Microsoft.Extensions.Logging;
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
        public IcmpPingService(IAddressLookupService addressLookupService, ILoggerFactory loggerFactory) : base(addressLookupService, loggerFactory.CreateLogger<DnsPingServiceBase>())
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
