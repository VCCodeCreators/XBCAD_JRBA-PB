using JRBAWebApplication2.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;


namespace JRBAWebApplication2.Controllers
{
	public class HomeController : Controller
	{
		/// <summary>
		/// Index View
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Board View
		/// </summary>
		/// <returns></returns>
		public ActionResult Dashboard()
		{
			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Estimations View
		/// </summary>
		/// <returns></returns>
		public ActionResult Estimations()
		{
			const decimal estimatedAmountDouble = 4235.34m;
			string estimatedAmount = "R" + estimatedAmountDouble + " p/m";
			ViewBag.EstimatedAmount = estimatedAmount;

			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Estimations page POST View
		/// </summary>
		/// <param name="model"></param>
		/// <param name="form"></param>
		/// <returns></returns>
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
					//model.cropSize = Convert.ToDouble(form.AllKeys.Contains("cropSize") ? form["cropSize"] : null);
					string basin = model.BasinSelection;
					string crop = model.CropSelection;
					double cropSize = model.cropSize;
					string estimatedAmount = "";
					if (double.TryParse(form["cropSize"], out cropSize) && !cropSize.ToString().IsNullOrWhiteSpace()) 
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

						//System.Diagnostics.Debug.WriteLine("water data: " + WaterDuty);

						// Calculate the estimate
						double estimate = WaterDuty * cropSize;
						estimatedAmount = "R" + estimate.ToString() + " p/m";
						// Store the estimate in the model
						model.FinalCalc = estimate;

						//cropSize can't be negative number
						if ((crop.Equals("Crop") || (basin.Equals("basin"))) || cropSize.ToString().IsNullOrWhiteSpace() || cropSize <=0)
						{
							// Handle the case where required fields are empty
							estimatedAmount = " Error: Invalid Input";

						}


						// Calculate the estimated amount as a formatted string
						//string estimatedAmount = "R" + estimate.ToString() + " p/m";
						ViewBag.EstimatedAmount = estimatedAmount;
					}
					else
					{
						estimatedAmount = " Error: Invalid Input";
						ViewBag.EstimatedAmount = estimatedAmount;


					}
				}
			}
			catch (Exception ex)
			{
				//outputs exception to ViewBag
				ViewBag.EstimatedAmount = ex.ToString();

			}
			// Return to the Estimations view with the updated model
			return View("Estimations", model);
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Material Page View
		/// </summary>
		/// <returns></returns>
		public ActionResult Material()
		{
			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Settings page View
		/// </summary>
		/// <returns></returns>
		public ActionResult Settings()
		{
			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// UploadMaterial View
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "Admin")]
		public ActionResult UploadMaterial()
		{
			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Drought information page view
		/// </summary>
		/// <returns></returns>
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
		/// <summary>
		/// Dashboard View
		/// </summary>
		/// <returns></returns>
		public ActionResult Dash()
		{
			ViewBag.Name = "";
			ViewBag.Comment = "";
			return View();
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// About Page View
		/// </summary>
		/// <returns></returns>
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Contacts Page View
		/// </summary>
		/// <returns></returns>
		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
	}
}
//------------------------------------------------End of File----------------------------------------------------\\
