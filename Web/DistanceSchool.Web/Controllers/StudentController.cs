using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistanceSchool.Web.Controllers
{
    public class StudentController : BaseController
    {
        public IActionResult OneStudent(string id)
        {
            return this.View();
        }
    }
}
