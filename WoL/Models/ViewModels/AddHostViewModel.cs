using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WoL.Extensions;

namespace WoL.Models.ViewModels
{
    public class AddHostViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Display(Name = "Hostname")]
        public string Hostname { get; set; }

        [StringLength(255)]
        [Required]
        [Display(Name = "Title")]
        public string Caption { get; set; }

        [StringLength(17, MinimumLength = 17)]
        [Display(Name = "Mac address")]
        public string MacAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MacAddress) && string.IsNullOrWhiteSpace(Hostname))
                yield return new ValidationResult("You must enter either a hostname or a mac address.", new[] { nameof(Hostname) });
            if (!string.IsNullOrWhiteSpace(MacAddress) && !string.IsNullOrWhiteSpace(Hostname))
                yield return new ValidationResult("You must enter either a hostname or a mac address, but not both.", new[] { nameof(Hostname) });
            if (!string.IsNullOrEmpty(MacAddress) && !MacAddress.TryParseMacAddress(out _))
                yield return new ValidationResult("The entered mac address is not valid.", new[] { nameof(MacAddress) });
        }
    }
}
