using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.ResonseModels
{
    public class UserProfileRequest
    {
        public string PhoneNumber { get; set; }

        // UserProfile
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public AddressResponse Address { get; set; }
    }
}