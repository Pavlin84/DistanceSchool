namespace DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute
{
    using System.Security.Claims;

    using DistanceSchool.Services;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Primitives;

    public class TeamPassportAcssesAttribute : AdminManagerAuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var teacherTeamResult = context.HttpContext.Request.Query.TryGetValue("TeacherTheamId", out StringValues teacherTheamId);

            var service = context.HttpContext.RequestServices.GetService(typeof(ICheckRoleAttributeService)) as ICheckRoleAttributeService;

            if (teacherTeamResult && service.CheckUserIsUserWithTeacherTeamId(userId, int.Parse(teacherTheamId)))
            {
                return;
            }

            base.OnAuthorization(context);
        }
    }
}
