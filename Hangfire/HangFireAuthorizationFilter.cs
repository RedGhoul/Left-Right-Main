using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LeftRightNet.Hangfire
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            Microsoft.AspNetCore.Http.HttpContext httpContext = context.GetHttpContext();
            if (httpContext == null) return false;
            string useRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (useRole == null) return false;
            return useRole.Equals("Admin");
        }
    }
}
