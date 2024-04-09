using Microsoft.AspNetCore.Authorization;

namespace AuthAPI.Authorization
{
    public class Role : IAuthorizationRequirement
    {
        public string RoleName { get; set; }

        public Role(string roleName)
        {
            RoleName = roleName;
        }
    }
}
