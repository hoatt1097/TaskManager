using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.Helpers
{
    public class CurrentUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Code { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class UserHelper
    {
        public static CurrentUser GetCurrentUser(HttpContext httpContext)
        {
            var claims = httpContext.User.Claims.ToList();
            var currentUser = new CurrentUser
            {
                Id = claims?.FirstOrDefault(x => x.Type.Equals("Id", StringComparison.OrdinalIgnoreCase))?.Value,
                Username = claims?.FirstOrDefault(x => x.Type.Equals("Username", StringComparison.OrdinalIgnoreCase))?.Value,
                Code = claims?.FirstOrDefault(x => x.Type.Equals("Code", StringComparison.OrdinalIgnoreCase))?.Value,
                Fullname = claims?.FirstOrDefault(x => x.Type.Equals("Fullname", StringComparison.OrdinalIgnoreCase))?.Value,
                Email = claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value,
                RoleId = claims?.FirstOrDefault(x => x.Type.Equals("RoleId", StringComparison.OrdinalIgnoreCase))?.Value,
                RoleName = claims?.FirstOrDefault(x => x.Type.Equals("RoleName", StringComparison.OrdinalIgnoreCase))?.Value,
            };
            return currentUser;
        }
    }
}
