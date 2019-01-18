using System.Collections.Generic;

namespace locationtrackapi.DAL
{
    public interface IUserDAL
    {
         IEnumerable<LoginModel> GetUsers();
    }
}