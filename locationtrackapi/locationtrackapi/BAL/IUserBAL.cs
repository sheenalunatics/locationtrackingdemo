using System.Collections.Generic;

namespace locationtrackapi.BAL
{
    public interface IUserBAL
    {
        IEnumerable<LoginModel> GetUsers();
        // IEnumerable<LoginModel> GetUserSites(LoginModel user);
        // LoginModel GetUserCurrentSite(LoginModel user);
    }
}