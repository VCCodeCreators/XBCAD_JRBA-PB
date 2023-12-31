﻿using JRBAWebApplication2.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Identity;
using System.Net;
using Azure.Storage.Blobs.Models;
using System.Data.Entity;

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
			decimal estimatedAmountDouble = 00.00m;
			string estimatedAmount = "R" + estimatedAmountDouble + " p/a";
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
						if ((crop.Equals("Crop") || (basin.Equals("basin"))) || cropSize.ToString().IsNullOrWhiteSpace() || cropSize <= 0)
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
		/// SaveEstimations
		/// </summary>
		/// <param name="model"></param>
		/// <param name="form"></param>
		/// <returns></returns>
		[Authorize]
		public ActionResult SaveEstimations(CalculationModels model, FormCollection form)
		{
			try
			{
				model.BasinSelection = form.AllKeys.Contains("BasinSelection") ? form["BasinSelection"] : null;
				model.CropSelection = form.AllKeys.Contains("CropSelection") ? form["CropSelection"] : null;
				model.cropSize = form.AllKeys.Contains("cropSize") ? Convert.ToDouble(form["cropSize"]) : 0.0;
				model.SiteName = form.AllKeys.Contains("SiteName") ? form["SiteName"] : null;
				model.Purpose = form.AllKeys.Contains("Description") ? form["Description"] : null;
				string basin = model.BasinSelection;
				string crop = model.CropSelection;
				string siteName = model.SiteName;
				string purpose = model.Purpose;
				double cropSize = model.cropSize;

				using (var dbContext = new ApplicationDbContext())
				{
					CalculationModels record = new CalculationModels
					{
						// Assign values to properties based on your data model
						CalculationId = Guid.NewGuid().ToString(),
						//UserId = User.Identity.GetUserId(),
						SiteName = siteName,
						Purpose = purpose,
						BasinSelection = basin,
						CropSelection = crop,
						cropSize = cropSize,
						FinalCalc = model.FinalCalc
					};
					// Add the new record to the DbSet
					//dbContext.SavedCalcs.Add(record);

					// Save changes to the database
					//dbContext.Entry(record).State = EntityState.Modified;
					dbContext.SaveChanges();
					ViewBag.EstimatedAmount = "Calculation Saved!";


				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
				ViewBag.EstimatedAmount = ex.ToString();
				return View("Estimations", model);


			}
			return View("Estimations", model);

		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Material Page View
		/// </summary>
		/// <returns></returns>
		//List<string> blobNames = new List<string>();

		public ActionResult Material()
		{
			var blobNames = new List<string>();

			try
			{
				string connectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
				BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

				string containerName = "jrba-blob";
				BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

			
				var blobs = containerClient.GetBlobs();

				// Loop through the blobs and add their names to the list
				foreach (var blob in blobs)
				{
					blobNames.Add(blob.Name);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("An error occurred while fetching blob names: " + ex.Message);
			}

			return View(blobNames);
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Download file
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public async Task<FileResult> DownloadFile(string fileName)
		{
			var stream = new MemoryStream();

			try
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					string connectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
					BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

					string containerName = "jrba-blob";
					BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

					BlobClient blobClient = containerClient.GetBlobClient(fileName);

					if (await blobClient.ExistsAsync())
					{
						stream = new MemoryStream();
						blobClient.DownloadTo(stream);
						stream.Seek(0, SeekOrigin.Begin);
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("The file does not exist in the blob storage.");
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("An error occurred while downloading the file" + ex);
			}

			return File(stream, "application/octet-stream", fileName);
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
		//uncomment to add role based access control
		[Authorize(Roles = "Admin")]
		public ActionResult UploadMaterial()
		{
			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// Upload Material POST
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> UploadMaterial(HttpPostedFileBase file)
		{
			string connectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
			BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

			try
			{
				if (file != null && file.ContentLength > 0)
				{
					string containerName = "jrba-blob";
					string blobName = Path.GetFileName(file.FileName);
					BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
					BlobClient blobClient = containerClient.GetBlobClient(blobName);

					using (Stream stream = file.InputStream)
					{
						await blobClient.UploadAsync(stream, true);
					}

					// File uploaded successfully
					System.Diagnostics.Debug.WriteLine("Upload = good");
					return RedirectToAction("Material");

				}
				else
				{
					// No file was selected for upload
					System.Diagnostics.Debug.WriteLine("Upload = bad");
					return View();
				}
			}
			catch (Exception ex)
			{
				var errorMessage = ex.Message.ToString();
				System.Diagnostics.Debug.WriteLine("problem is here:" + errorMessage);
				return View();
			}
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
