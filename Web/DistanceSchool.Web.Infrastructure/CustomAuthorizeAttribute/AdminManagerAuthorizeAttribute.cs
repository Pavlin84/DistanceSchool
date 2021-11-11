namespace DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute
{
    using System.Security.Claims;

    using DistanceSchool.Common;
    using DistanceSchool.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Primitives;

    public class AdminManagerAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return;
            }

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var schoolResult = context.HttpContext.Request.Query.TryGetValue("id", out StringValues schoolId);
            var candidacuResult = context.HttpContext.Request.Query.TryGetValue("id", out StringValues candidacyId);
            var teacherResult = context.HttpContext.Request.Query.TryGetValue("TeacherId", out StringValues teacherId);
            var teamResult = context.HttpContext.Request.Query.TryGetValue("TeamId", out StringValues teamId);
            var teacherTheamResult = context.HttpContext.Request.Query.TryGetValue("TeacherTheamId", out StringValues teacherTheamId);

            // if(!teacherResult && context.HttpContext.Request.HasFormContentType)
            // {
            //     teacherResult = context.HttpContext.Request.Form.TryGetValue("TeacherId", out teacherId);
            // }

            var hasResult = schoolResult || candidacuResult || teacherResult || teamResult || teacherTheamResult;

            var service = context.HttpContext.RequestServices.GetService(typeof(ICheckRoleAttributeService)) as ICheckRoleAttributeService;

            if (!hasResult && service.CheckUserIsManager(userId))
            {
                return;
            }

            if (teacherResult && service.CheckUserIsManagerWithTeacherId(userId, teacherId))
            {
                return;
            }

            if (schoolResult && service.CheckUserIsManager(userId, int.Parse(schoolId)))
            {
                return;
            }

            if (candidacuResult && service.CheckUserIsManagerToCandidacy(userId, int.Parse(candidacyId)))
            {
                return;
            }

            if (teamResult && service.CheckUserIsManagerWithTeamId(userId, int.Parse(teamId)))
            {
                return;
            }

            if (teacherTheamResult && service.CheckUserIsManagerWithTeacherTeamId(userId, int.Parse(teacherTheamId)))
            {
                return;
            }

            context.Result = new RedirectResult("/Identity/Account/AccessDenied");
        }

    }
}
