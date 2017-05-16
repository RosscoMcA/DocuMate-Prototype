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
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();
           
            
            return View();
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