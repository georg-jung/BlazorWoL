using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WoL.Extensions;

namespace WoL.Models
{
    public class Host
    {
        public int Id { get; set; }
        
        [StringLength(255)]
        public string Hostname { get; set; }
        
        [StringLength(255)]
        [Required]
        public string Caption { get; set; }

        [MaxLength(6)]
        [Required]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Eigenschaften dürfen keine Arrays zurückgeben", Justification = "A mac address intrinsically is a byte[]")]
        public byte[] MacAddress { get; set; }

        public override string ToString()
        {
            return $"Host(Id = {Id}; Hostname = {Hostname}; MacAddress = {(MacAddress == null ? "null" : this.GetMacString())}; Caption = {Caption})";
        }
    }
}
