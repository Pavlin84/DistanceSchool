namespace DistanceSchool.Web.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DistanceSchool.Services.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Primitives;

    public class AddDisciplinesViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDisciplineService disciplineService;

        public AddDisciplinesViewComponent(IDisciplineService disciplineService, IHttpContextAccessor httpContext)
        {
            this.httpContextAccessor = httpContext;
            this.disciplineService = disciplineService;
        }

        public IViewComponentResult Invoke(int id)
        {

            var disciplines = this.disciplineService.GetSchoolDisciplines(id);
            var viewModel = new AddDisciplinesViewModel
            {
                DisciplinesId = disciplines.Select(x => new SelectListItem
                {
                    Text = x.Key,
                    Value = x.Value.ToString(),
                }).ToList(),
            };
            return this.View(viewModel);
        }
    }
}
