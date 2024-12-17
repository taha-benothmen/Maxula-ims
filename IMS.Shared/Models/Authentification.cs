using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Shared.Models
{

    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    // Request Model for User Sign-Up
    public class UserSignUpRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // Request Model for User Sign-In
    public class UserSignInRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

   
}
