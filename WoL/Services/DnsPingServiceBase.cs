using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        public DnsPingServiceBase(IAddressLookupService addressLookupService, ILogger logger)
        {
            this.addressLookupService = addressLookupService;
            this.logger = logger;
        }

        public async Task<IPingService.PingResult> IsReachable(string hostname, TimeSpan timeout)
        {
            IPAddress ip;
            try
            {
                (ip, _) = await addressLookupService.GetIpAndName(hostname).ConfigureAwait(false);
            }
            // could throw at least SocketException, ArgumentException, ArgumentOutOfRangeException, InvalidOperationException, maybe NRE, which would be of interest
            // we want to catch them all, basically anything that can make the resolution fail
#pragma warning disable CA1031 // Keine allgemeinen Ausnahmetypen abfangen
            catch (Exception ex)
#pragma warning restore CA1031 // Keine allgemeinen Ausnahmetypen abfangen
            {
                logger.LogDebug(ex, $"Exception during {nameof(IAddressLookupService)}.{nameof(IAddressLookupService.GetIpAndName)}");
                return PingResult.HostNotFound;
            }
            return await IsReachable(ip, timeout).ConfigureAwait(false) ? PingResult.Success : PingResult.Unreachable;
        }

        public abstract Task<bool> IsReachable(IPAddress ip, TimeSpan timeout);
    }
}
