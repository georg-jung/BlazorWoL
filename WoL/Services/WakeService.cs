using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WoL.Services
{
    public class WakeService : IWakeService
    {
        public Task Wake(byte[] mac)
        {
            return IPAddress.Broadcast.SendWolAsync(mac, 40000);
        }
    }
}
