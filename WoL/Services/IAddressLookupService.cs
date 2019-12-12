using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace WoL.Services
{
    public interface IAddressLookupService
    {
        Task<(IPAddress, string)> GetIpAndName(string hostname);
        Task<PhysicalAddress> GetMac(IPAddress ip);
    }
}