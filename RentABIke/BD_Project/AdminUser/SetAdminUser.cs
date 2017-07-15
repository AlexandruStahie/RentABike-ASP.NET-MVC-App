using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BD_Project.AdminUser
{
    public static class SetAdminUser
    {
        public static bool IsAdmin(this IPrincipal user)
        {
            //if (user == ) ;
            return user.IsInRole("admin");
        }

    }
}