using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App.ResonseModels.RequestModels
{
    public class UpdateUserProfileRequest
    {
        // UserProfile
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }

    public class UpdateAddressRequest
    {
        public string Text { get; set; }

        [Required]
        public string Line { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }
    }
}