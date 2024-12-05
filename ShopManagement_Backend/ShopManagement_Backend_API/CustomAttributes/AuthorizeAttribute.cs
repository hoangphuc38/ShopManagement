using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_Application.Models;

namespace ShopManagement_Backend_API.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var user = (User)context.HttpContext.Items["User"];

            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var rolesAttribute = context.ActionDescriptor.EndpointMetadata.OfType<RoleAttribute>().FirstOrDefault();
            if (rolesAttribute == null)
                return;

            if (rolesAttribute.Roles.Contains(user.Role.RoleName))
                return;

            context.Result = new JsonResult(new { message = "Invalid authorized to access" }) { StatusCode = StatusCodes.Status403Forbidden };
            return;
        }
    }
}
