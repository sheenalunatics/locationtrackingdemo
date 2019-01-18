using System.Collections.Generic;

namespace locationtrackapi
{
    public class UserHelper
    {
        public IEnumerable<LoginModel> GetUsers()
        {
             //IEnumerable<LoginModel> users;
             List<LoginModel> users = new List<LoginModel>();
            // using (var usrContext = new ApplicationDbContext())
            // {
            //     StringBuilder sql = new StringBuilder();
            //     sql.Append("SELECT urs.Id,urs.UserName,urs.Email,ord.Password,ord.PasswordFormat,ord.HashAlgorithm,ord.PasswordSalt,ord.EmplId,ord.FullName");
            //     sql.Append(" FROM [" + usrContext.Database.Connection.Database + "].[dbo].[AspNetUsers] urs");
            //     sql.Append(" LEFT JOIN " + OrchardUserAddress + " ord");
            //     sql.Append(" ON urs.UserName = ord.UserName");
            //     sql.Append(" WHERE urs.LockoutEnabled = 0");
            //     users = usrContext.Database.SqlQuery<UserOrdchardModel>(sql.ToString()).ToList();
            // }
            LoginModel user = new LoginModel
                                        {
                                            ApplicationID = "3995132E-22B0-493E-A4BF-2FF52509FAF9",Password="1MdU1J96tb8l3ouGwcQXBU3rMss=",
                                            UserType="api",UserName="ukrit.s",
                                            PasswordSalt="KFU/UTx8EDKxBX4ztAqP8Q==",
                                            Issuer="AuthenticationService"
                                            };

           users.Add(user);
            return users;
        }
    }
}