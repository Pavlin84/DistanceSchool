namespace DistanceSchool.Web.Areas.Administration.Controllers
{
    using DistanceSchool.Common;
    using DistanceSchool.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
