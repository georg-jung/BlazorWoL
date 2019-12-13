using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace WoL.Services
{
    public class AddressLookupService : IAddressLookupService
    {
        public async Task<(IPAddress, string)> GetIpAndName(string hostname)
        {
            var res = await Dns.GetHostEntryAsync(hostname).ConfigureAwait(false);
            return (res.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First(), res.HostName);
        }

        public async Task<PhysicalAddress> GetMac(IPAddress ip)
        {
            return await ArpLookup.Arp.LookupAsync(ip).ConfigureAwait(false);
        }
    }
}
