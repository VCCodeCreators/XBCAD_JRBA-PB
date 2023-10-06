using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace JRBAWebApplication2.Models
{
	public class UserModel
	{

		//[Required(ErrorMessage = "The UserRoles field is required.")]
		public string UserRoles { get; set; }

		// Primary key property
		[Key]
		public String UserId { get; set; } 

		public string FirstName { get; set; }

		public string LastName { get; set; }


		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "User Name")]
		public string UserName { get; set; }


		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
//---------------------------------------------End of File-------------------------------------------------------\\