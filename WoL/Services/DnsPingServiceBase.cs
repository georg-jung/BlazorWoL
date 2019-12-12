using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static WoL.Services.IPingService;

namespace WoL.Services
{
    public abstract class DnsPingServiceBase : IPingService
    {
        private readonly IAddressLookupService addressLookupService;

        public DnsPingServiceBase(IAddressLookupService addressLookupService)
        {
            this.addressLookupService = addressLookupService;
        }

        public async Task<IPingService.PingResult> IsReachable(string hostname, TimeSpan timeout)
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

        public abstract Task<bool> IsReachable(IPAddress ip, TimeSpan timeout);
    }
}
