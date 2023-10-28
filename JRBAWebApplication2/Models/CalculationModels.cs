using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JRBAWebApplication2.Models
{
	public class CalculationModels
	{

		// Primary key property
		[Key]
		public String CalculationId { get; set; }

		// Foreign key property
		[ForeignKey("UserModel")]
		public string UserId { get; set; }
		public UserModel UserModel { get; set; }


		//public string CalculationName { get; set; }
		public string Purpose { get; set; }
		public string SiteName { get; set; }
		//public DateTime CalculationDate { get; set; }
		public string BasinSelection { get; set; }
		public string CropSelection { get; set; }
		public double cropSize { get; set;}
		public double DesiredYield { get; set;}
		public double FinalCalc { get; set; }

	}


}
//---------------------------------------------End of File-------------------------------------------------------\\