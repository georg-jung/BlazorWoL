using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WoL.Extensions;

namespace WoL.Models.ViewModels
{
    public class HostViewModel
    {
        public HostViewModel()
        {
        }

        public HostViewModel(Host host)
        {
            Id = host.Id;
            Hostname = host.Hostname;
            Caption = host.Caption;
            MacAddress = host.GetMacString();
            Status = string.IsNullOrEmpty(Hostname) ? HostStatus.NoHostname : HostStatus.Loading;
        }

        public int Id { get; set; }

        public string Hostname { get; set; }

        public string Caption { get; set; }

        public string MacAddress { get; set; }

        public HostStatus Status { get; set; }

        public enum HostStatus
        {
            Loading,
            Online,
            Unreachable,
            HostnameInvalid,
            NoHostname
        }
    }
}
