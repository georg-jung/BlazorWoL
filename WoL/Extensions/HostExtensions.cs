using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WoL.Models;

namespace WoL.Extensions
{
    public static class HostExtensions
    {
        public static string GetMacString(this Host value)
        {
            var adrBytes = value?.MacAddress ?? throw new ArgumentNullException(nameof(value));
            return string.Join(":", from z in adrBytes select z.ToString("X2", CultureInfo.InvariantCulture));
        }
    }
}
