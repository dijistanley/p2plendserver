using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App.ResonseModels.RequestModels
{
    public class UpdateAccountRequest
    {
    }

    public class UpdateEmailRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UpdatePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }

    public class UpdatePhoneRequest
    {
        [Required]
        public string NewPhoneNumber { get; set; }
    }
}