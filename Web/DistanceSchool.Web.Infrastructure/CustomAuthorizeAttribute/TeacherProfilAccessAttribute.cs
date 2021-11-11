namespace DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute
{
    using System.Security.Claims;

    using DistanceSchool.Services;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Primitives;

    public class TeacherProfilAccessAttribute : AdminManagerAuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var schoolResult = context.HttpContext.Request.Query.TryGetValue("TeacherId", out StringValues teacherId);

            var service = context.HttpContext.RequestServices.GetService(typeof(ICheckRoleAttributeService)) as ICheckRoleAttributeService;

            if (service.CheckIsUser(userId, teacherId))
            {
                return;
            }

            base.OnAuthorization(context);
        }
    }
}
