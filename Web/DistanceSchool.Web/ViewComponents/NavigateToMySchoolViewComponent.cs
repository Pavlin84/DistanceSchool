namespace DistanceSchool.Web.ViewComponents
{
    using DistanceSchool.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class NavigateToMySchoolViewComponent : ViewComponent
    {
        private readonly ISchoolService schoolService;

        public NavigateToMySchoolViewComponent(ISchoolService schoolService)
        {
            this.schoolService = schoolService;
        }

        public IViewComponentResult Invoke(string id)
        {
            var viewModel = this.schoolService.GetSchoolIdByUserId(id);

            return this.View(viewModel);
        }
    }
}
