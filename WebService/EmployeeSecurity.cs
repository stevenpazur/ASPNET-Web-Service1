using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserDataAccess;

namespace WebService
{
    public class EmployeeSecurity
    {
        public static bool Login(string username, string password)
        {
            using (UserDBEntities entities = new UserDBEntities())
            {
                return entities.Mods.Any(user => user.Username.Equals(username,
                    StringComparison.OrdinalIgnoreCase) && user.Password == password);
            }
        }
    }
}