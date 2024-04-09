using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AuthAPI.Authorization
{
    public class RoleAuthorization : AuthorizationHandler<Role>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Role requirement)
        {
            var roleClaim = context.User.Claims
                            .FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

            if (roleClaim is null) return Task.CompletedTask;

            if(roleClaim.Value == requirement.RoleName)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
