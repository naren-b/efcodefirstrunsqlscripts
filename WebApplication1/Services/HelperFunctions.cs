using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebApplication1.Services
{
    public static class HelperFunctions
    {
        public static string GetLoggedInUserName()
        {
            if (HttpContext.Current?.User is WindowsPrincipal) //Windows authentication
                return HttpContext.Current?.Request?.LogonUserIdentity?.Name;
            else //Forms authentication
                return HttpContext.Current?.User?.Identity?.Name;
        }
    }
}