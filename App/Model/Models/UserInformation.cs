﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class UserInformation
    {

        [Key]
        public string Id { get; set; }
        
        public string UserId { get; set; }

        public Address Address { get; set; }
    }
}
