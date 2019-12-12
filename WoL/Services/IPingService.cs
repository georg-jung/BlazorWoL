using System;
using System.Net;
using System.Threading.Tasks;

namespace WoL.Services
{
    public interface IPingService
    {
        Task<bool> IsReachable(IPAddress ip, TimeSpan timeout);
        Task<PingResult> IsReachable(string hostname, TimeSpan timeout);

        public enum PingResult
        {
            Unreachable,
            HostNotFound,
            Success
        }
    }
}