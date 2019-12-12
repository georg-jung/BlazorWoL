using System.Net;
using System.Threading.Tasks;

namespace WoL.Services
{
    public interface IPingService
    {
        Task<bool> IsReachable(IPAddress ip, int timeout);
        Task<PingResult> IsReachable(string hostname, int timeout);

        public enum PingResult
        {
            Unreachable,
            HostNotFound,
            Success
        }
    }
}