using JRBAWebApplication2.Models;
using Microsoft.Ajax.Utilities;
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
		//----------------------------------------------------------------------------------------------------\\

		public ActionResult Dashboard()
        {
            return View();
        }
		//----------------------------------------------------------------------------------------------------\\

		public ActionResult Estimations()
        {
			
			int WaterDuty = 0;
					var EstimationModel = new CalculationModels();
            if (EstimationModel.CropType.Equals( "Sugarcane") && EstimationModel.Location.Equals( "Komati/Lomati"))
            {
                 WaterDuty = 12000;
            }
			else if (EstimationModel.CropType.Equals( "Sugarcane") && EstimationModel.Location.Equals( "Malkerns"))
			{
				 WaterDuty = 10880;
			}
			else if (EstimationModel.CropType.Equals( "Sugarcane") && EstimationModel.Location.Equals( "Usuthu/Other"))
			{
				 WaterDuty = 13650;
			}
			else if (EstimationModel.CropType.Equals( "Other"))
			{
				 WaterDuty = 8000;
			}
			//Water use estimation = water duty for crop (M^3/ha) x Area to be farmed (ha).
			double estimate = WaterDuty * EstimationModel.CropSize;  
			decimal estimatedAmountDouble = 4235.34m;
            string estimatedAmount = "R"+ estimatedAmountDouble+" p/m";
			ViewBag.EstimatedAmount = estimatedAmount;

			return View();
        }
		//----------------------------------------------------------------------------------------------------\\
		/*[HttpPost]
		public ActionResult Estimations(CalculationModels model)
		{
			if (ModelState.IsValid)
			{
				// Perform the calculation and update the WaterUseEstimation property
				//model.FinalCalc = model * model.AreaToFarm;
			}

			// Return to the Estimations view with the updated model
			return View("Estimations", model);
		}*/
		//----------------------------------------------------------------------------------------------------\\

		public ActionResult Material()
        {
            return View();
        }
		//----------------------------------------------------------------------------------------------------\\

		public ActionResult Settings()
        {
            return View();
        }
		//----------------------------------------------------------------------------------------------------\\

		public ActionResult UploadMaterial()
        {
            return View();
        }
		//----------------------------------------------------------------------------------------------------\\

		public ActionResult DroughtView()
        {
			//DroughtView Outputs
			ViewBag.PromoName = "";
			ViewBag.StartDate = "";
			ViewBag.EndDate = "";
			ViewBag.DroughtLevel = "";
			ViewBag.Comments = "";


			return View();
        }
		//----------------------------------------------------------------------------------------------------\\

		public ActionResult Dash()
        {
			ViewBag.Name = "";
			ViewBag.Comment = "";
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
		//----------------------------------------------------------------------------------------------------\\
	}
}
//------------------------------------------------End of File----------------------------------------------------\\
