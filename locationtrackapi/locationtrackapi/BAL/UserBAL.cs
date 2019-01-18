using System.Collections.Generic;
using locationtrackapi.DAL;

namespace locationtrackapi.BAL
{
    public class UserBAL : IUserBAL
    {
        private IUserDAL _objUserDal;

       public UserBAL(IUserDAL objUserDal)
        {
            _objUserDal = objUserDal;
        }

        public IEnumerable<LoginModel> GetUsers()
        {
            return _objUserDal.GetUsers();
        }
    }
}