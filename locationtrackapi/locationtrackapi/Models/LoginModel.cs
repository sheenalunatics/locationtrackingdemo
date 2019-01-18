using System.Collections.Generic;

namespace locationtrackapi
{
    public class LoginModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public LoginModel()
        {
            Roles = new List<string>();
        }
       // public string Email { get; set; }
    }
}