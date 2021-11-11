using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistanceSchool.Web.ViewComponents
{
    public class InvokePrivacyAlertViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {

            var viewModel = this.HttpContext.Session.GetString("LookPrivacy") == null
                 ? false
                 : JsonConvert.DeserializeObject<bool>(this.HttpContext.Session.GetString("LookPrivacy"));
            return this.View(viewModel);
        }
    }
}
