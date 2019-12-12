using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WoL.Models.ViewModels;
using WoL.Services;

namespace WoL.Extensions
{
    public static class PingServiceExtensions
    {
        public static HostViewModel.HostStatus ToHostStatus(this IPingService.PingResult result)
        {
            return result switch
            {
                IPingService.PingResult.Success => HostViewModel.HostStatus.Online,
                IPingService.PingResult.Unreachable => HostViewModel.HostStatus.Unreachable,
                IPingService.PingResult.HostNotFound => HostViewModel.HostStatus.HostnameInvalid,
                _ => throw new NotImplementedException()
            };
        }
    }
}
