using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntermediaryBuz.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public JsonResult SubmitForm(string json)
		{
			var obj = JsonConvert.DeserializeObject(json);
			return Json(new { msgId=1, msgContent="success", data="good" });
		}
    }
}