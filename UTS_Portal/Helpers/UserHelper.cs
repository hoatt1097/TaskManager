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
                Id = claims?.FirstOrDefault(x => x.Type.Equals("Id", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                Username = claims?.FirstOrDefault(x => x.Type.Equals("Username", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                Code = claims?.FirstOrDefault(x => x.Type.Equals("Code", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                Fullname = claims?.FirstOrDefault(x => x.Type.Equals("Fullname", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                Email = claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                RoleId = claims?.FirstOrDefault(x => x.Type.Equals("RoleId", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                RoleName = claims?.FirstOrDefault(x => x.Type.Equals("RoleName", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
            };
            return currentUser;
        }
    }
}
