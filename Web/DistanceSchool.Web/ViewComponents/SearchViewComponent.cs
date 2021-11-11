namespace DistanceSchool.Web.ViewComponents
{

    using DistanceSchool.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class SearchViewComponent : ViewComponent
    {
        private readonly IDisciplineService disciplineService;

        public SearchViewComponent(IDisciplineService disciplineService)
        {
            this.disciplineService = disciplineService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = this.disciplineService.GetAllDisciplines();
            return this.View(viewModel);
        }
    }
}
