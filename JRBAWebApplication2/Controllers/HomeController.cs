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
			decimal estimatedAmountDouble = 4235.34m;
			string estimatedAmount = "R" + estimatedAmountDouble + " p/m";
			ViewBag.EstimatedAmount = estimatedAmount;

			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		[HttpPost]
		public ActionResult Estimations(CalculationModels model, FormCollection form)
		{
			// Debugging code \\
			//System.Diagnostics.Debug.WriteLine("BasinSelection: " + model.BasinSelection);
			//System.Diagnostics.Debug.WriteLine("CropSelection: " + model.CropSelection);
			//System.Diagnostics.Debug.WriteLine("Form data: " + form);
			try
			{
				if (ModelState.IsValid)
				{

					model.BasinSelection = form.AllKeys.Contains("BasinSelection") ? form["BasinSelection"] : null;
					model.CropSelection = form.AllKeys.Contains("CropSelection") ? form["CropSelection"] : null;
					string basin = model.BasinSelection;
					string crop = model.CropSelection;
					double cropSize = model.cropSize;
					string estimatedAmount = "";
					if (double.TryParse(form["cropSize"], out cropSize) && !cropSize.Equals(null))
					{
						double WaterDuty = 0;

						if (crop.Equals("Sugar Cane") && (basin.Equals("Komati") || basin.Equals("Lomati")))
						{
							WaterDuty = 12000;
						}
						else if (crop.Equals("Sugar Cane") && basin.Equals("Malkerns"))
						{
							WaterDuty = 10880;
						}
						else if (crop.Equals("Sugar Cane") && (basin.Equals("Usuthu") || basin.Equals("Other") || basin.Equals("Ngwavuma")))
						{
							WaterDuty = 13650;
						}
						else if (!string.IsNullOrEmpty(crop) || crop.Equals("other"))
						{
							WaterDuty = 8000;
						}

						System.Diagnostics.Debug.WriteLine("Form data: " + WaterDuty);

						// Calculate the estimate
						double estimate = WaterDuty * cropSize;
						estimatedAmount = "R" + estimate.ToString() + " p/m";
						// Store the estimate in the model
						model.FinalCalc = estimate;

						if ((crop.Equals("Crop") || (basin.Equals("basin"))) || cropSize.Equals(0) || cropSize.Equals(null))
						{
							// Handle the case where required fields are empty
							estimatedAmount = "Error";

						}


						// Calculate the estimated amount as a formatted string
						//string estimatedAmount = "R" + estimate.ToString() + " p/m";
						ViewBag.EstimatedAmount = estimatedAmount;
					}
					else
					{
						estimatedAmount = "Error";
						ViewBag.EstimatedAmount = estimatedAmount;


					}
				}
			}
			catch (Exception ex)
			{
				//outputs exception to ViewBag
				ViewBag.EstimatedAmount = ex;

			}
			// Return to the Estimations view with the updated model
			return View("Estimations", model);
		}

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
