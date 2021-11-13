namespace DistanceSchool.Web.Controllers
{
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using Microsoft.AspNetCore.Mvc;

    public class ChatController : BaseController
    {

        [TeamAccessAuthorize]
        public IActionResult ChatStart(int teamId)
        {
            return this.View();
        }
    }
}
