using System;
using System.Collections.Generic;
using System.Text;

namespace webShop2.model
{
    public class User : Entity
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Admin";

        public bool IsActive { get; set; } = true;
    }
}
