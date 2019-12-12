using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using static WoL.Services.IPingService;

namespace WoL.Services
{
    public class PingService : IPingService
    {
        private IAddressLookupService addressLookupService;

        public PingService(IAddressLookupService addressLookupService)
        {
            this.addressLookupService = addressLookupService;
        }

        public async Task<PingResult> IsReachable(string hostname, int timeout)
        {
            IPAddress ip;
            try
            {
                (ip, _) = await addressLookupService.GetIpAndName(hostname).ConfigureAwait(false);
            }
            catch
            {
                return PingResult.HostNotFound;
            }
            return await IsReachable(ip, timeout).ConfigureAwait(false) ? PingResult.Success : PingResult.Unreachable;
        }

        public async Task<bool> IsReachable(IPAddress ip, int timeout)
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(ip, timeout).ConfigureAwait(false);
            return reply.Status == IPStatus.Success;
        }
    }
}
