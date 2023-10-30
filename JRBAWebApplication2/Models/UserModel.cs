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


		[Required(ErrorMessage = "First name is required.")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required.")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		public string PhoneNo { get; set; }

		public string StreetAddress { get; set; }

		public string City { get; set; }


		public string Province { get; set; }

		public string Country { get; set; }




		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "User Name")]
		public string UserName { get; set; }


		[Required(ErrorMessage = "Password is required.")]
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