using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Helpers
{
    public class CurrentUser
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string LastLogin { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
    }
    public class UserHelper
    {
        public static CurrentUser GetCurrentUser(HttpContext httpContext)
        {
            var claims = httpContext.User.Claims.ToList();
            var currentUser = new CurrentUser
            {
                UserId = claims?.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                Username = claims?.FirstOrDefault(x => x.Type.Equals("Username", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                DisplayName = claims?.FirstOrDefault(x => x.Type.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                LastLogin = claims?.FirstOrDefault(x => x.Type.Equals("LastLogin", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                ChannelId = claims?.FirstOrDefault(x => x.Type.Equals("ChannelId", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                ChannelName = claims?.FirstOrDefault(x => x.Type.Equals("ChannelName", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                RoleName = claims?.FirstOrDefault(x => x.Type.Equals("RoleName", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
                RoleId = claims?.FirstOrDefault(x => x.Type.Equals("RoleId", StringComparison.OrdinalIgnoreCase))?.Value?.Trim(),
            };
            return currentUser;
        }
    }
}
