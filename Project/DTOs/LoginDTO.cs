﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}