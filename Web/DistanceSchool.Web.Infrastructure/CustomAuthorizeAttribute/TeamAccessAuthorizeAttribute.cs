using DistanceSchool.Common;
using DistanceSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute
{
    public class TeamAccessAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return;
            }

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var schoolResult = context.HttpContext.Request.Query.TryGetValue("teamId", out StringValues teamId);

            var service = context.HttpContext.RequestServices.GetService(typeof(ICheckRoleAttributeService)) as ICheckRoleAttributeService;

            if (schoolResult && service.CheckHasAccessToTeam(userId, int.Parse(teamId)))
            {
                return;
            }

            context.Result = new RedirectResult("/Identity/Account/AccessDenied");
        }
    }
}
