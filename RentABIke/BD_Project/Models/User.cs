using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BD_Project.Models
{
    public class User
    {
        [Required(ErrorMessage = "Username-ul e obligatoriu")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Parola e obligatorie")]
        public string Password { get; set; }
    }
}