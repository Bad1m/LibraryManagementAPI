﻿using System.ComponentModel;

namespace LibraryManagementAPI.Models
{
    public class UserViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? Username { get; set; }
        [DefaultValue("john@example.com")]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
