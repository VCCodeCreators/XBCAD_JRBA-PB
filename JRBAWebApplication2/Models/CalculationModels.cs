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

		public string CalculationName { get; set; }

		public DateTime CalculationDate { get; set; }
		public string Location { get; set; }
		public string CropType { get; set; }
		public double CropSize { get; set;}
		public double DesiredYield { get; set;}
		public double FinalCalc { get; set; }

	}


}
//---------------------------------------------End of File-------------------------------------------------------\\