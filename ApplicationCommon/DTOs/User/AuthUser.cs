using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCommon.DTOs.User
{
    public class AuthUser
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
