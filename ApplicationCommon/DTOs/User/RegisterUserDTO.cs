
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCommon.DTOs.User
{
    public class RegisterUserDTO
    {
        public int UserId { get; }
        public string? Avatar { get; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Password { get; set; }
    }
}
