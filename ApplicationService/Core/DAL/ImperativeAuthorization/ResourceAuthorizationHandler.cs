using System.Threading.Tasks;
using ApplicationService.Core.Domain;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationService.Core.DAL.ImperativeAuthorization
{
    class ResourceAuthorizationHandler :
    AuthorizationHandler<SameAuthorRequirement, ShareableResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       SameAuthorRequirement requirement,
                                                       ShareableResource resource)
        {
            if (context.User.Identity?.Name == resource.OwnerId.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class SameAuthorRequirement : IAuthorizationRequirement { }
}
