using System.Collections.Generic;

namespace locationtrackapi.DAL
{
    public class UserDAL : IUserDAL
    {
        public IEnumerable<LoginModel> GetUsers()
        {
            return new UserHelper().GetUsers();
        }
    }
}