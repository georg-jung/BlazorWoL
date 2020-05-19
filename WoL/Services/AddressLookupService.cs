using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WoL.Services
{
    public class AddressLookupService : IAddressLookupService
    {
        public async Task<(IPAddress, string)> GetIpAndName(string hostname)
        {
            var res = await Dns.GetHostEntryAsync(hostname).ConfigureAwait(false);
            return (res.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork), res.HostName);
        }

        public async Task<PhysicalAddress> GetMac(IPAddress ip)
        {
            return await ArpLookup.Arp.LookupAsync(ip).ConfigureAwait(false);
        }
    }
}
