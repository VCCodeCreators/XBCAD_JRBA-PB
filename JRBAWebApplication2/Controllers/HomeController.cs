using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JRBAWebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Estimations()
        {
            return View();
        }
        public ActionResult Material()
        {
            return View();
        }
        public ActionResult Settings()
        {
            return View();
        }

        public ActionResult UploadMaterial()
        {
            return View();
        }
        public ActionResult DroughtView()
        {
            return View();
        }

        public ActionResult Dash()
        {
            return View();
        }

		//----------------------------------------------------------------------------------------------------\\
		public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
		//----------------------------------------------------------------------------------------------------\\
		public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
//------------------------------------------------End of File----------------------------------------------------\\
