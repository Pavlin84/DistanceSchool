using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistanceSchool.Web.Controllers
{
    public class ExamController : BaseController
    {
        public IActionResult ChangeStartTime(int id)
        {
            return this.View();
        }

        public IActionResult CreateExam(int id)
        {
            return this.View();
        }
    }
}
