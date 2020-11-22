using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GameForum.Entities;

namespace GameForum.Models
{
    public class AuthenticateResponse
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public UserRole role { get; set; }
        public string email { get; set; }
        public string JwtToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken)
        {
            id = user.id;
            username = user.username;
            password = user.password;
            role = user.role;
            JwtToken = jwtToken;
        }
    }
}
