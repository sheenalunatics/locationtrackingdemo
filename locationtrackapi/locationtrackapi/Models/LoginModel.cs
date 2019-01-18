using System.Collections.Generic;

namespace locationtrackapi
{
    public class LoginModel
    {
        public string ApplicationID { get; set; }
        public string Issuer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string UserType { get; set; }
        // public List<string> Roles { get; set; }
        // public LoginModel()
        // {
        //     Roles = new List<string>();
        // }
       // public string Email { get; set; }
    }
}