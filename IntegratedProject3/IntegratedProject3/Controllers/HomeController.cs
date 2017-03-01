using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntegratedProject3.Controllers
{
    public class HomeController : RootController
    {
        public ActionResult Index()
        {
            var account = getAccount();
            if (account != null) return View();
            else
            {

                return RedirectToAction("Login", "Account");

            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}