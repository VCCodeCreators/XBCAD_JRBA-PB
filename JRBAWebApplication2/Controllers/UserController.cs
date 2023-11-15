using JRBAWebApplication2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JRBAWebApplication2.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		//Property Decloration\\
		private readonly ApplicationDbContext db = new ApplicationDbContext();
		private ApplicationUserManager _userManager;
		readonly ApplicationDbContext context;



		public UserController()
		{
			context = new ApplicationDbContext();

		}
		//----------------------------------------------------------------------------------------------------\\
		public UserController(ApplicationUserManager userManager)
		{
			UserManager = userManager;

		}

		//----------------------------------------------------------------------------------------------------\\

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		//----------------------------------------------------------------------------------------------------\\
		// [ValidateAntiForgeryToken]
		//[HttpPost]
		//[Authorize]
		// GET: User
		public ActionResult Index()
		{
			// Check user role and set ViewBag.displayMenu accordingly
			if (User.IsInRole("Admin") || User.IsInRole("Common"))
			{
				ViewBag.displayMenu = "Yes";
			}
			else
			{
				ViewBag.displayMenu = "No";
			}

			// Set ViewBag.Name with the user's name if desired

			return View();
		}
		// GET: user/AddUser
		[AllowAnonymous]
		public ActionResult AddUser()
		{
			
			return View();
		}
		//----------------------------------------------------------------------------------------------------\\

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> AddUser(UserModel newUser)
		{
			var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = newUser.Email, Id = newUser.UserId };
				var result = await UserManager.CreateAsync(user, newUser.Password);
				if (result.Succeeded)
				{
					if (!await RoleManager.RoleExistsAsync("Common"))
					{
						await RoleManager.CreateAsync(new IdentityRole("Common"));
					}

					// Set the UserRoles property to "Common"
					newUser.UserRoles = "Common";

					newUser.UserId = User.Identity.GetUserId();
					newUser.Email = User.Identity.GetUserName();

					// Save the user to the database
					db.Users.Add(newUser);
					db.SaveChanges();

					return RedirectToAction("Index", "Users");
				}

				AddErrors(result);
			}

			// If we got this far, something failed, redisplay form    
			return View(newUser);
		}

		//-------------------------------------------------------------------------------------------------------------\\
		//[ValidateAntiForgeryToken]
		//[Authorize]
		public ActionResult AccessUserFunctions()
		{

			if (User.IsInRole("Employee"))
			{
				return RedirectToAction("Index", "User");
			}
			else
			{
				// Handle the case when the user is not in the "Employee" role
				return RedirectToAction("AccessDenied", "Error");
				// You can create an "Error" controller with an "AccessDenied" action
				// that displays an appropriate error message or redirects to another page.
			}


		}
		//-------------------------------------------------------------------------------------------------------------\\
		public Boolean IsAdminUser()
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = User.Identity;
				//ApplicationDbContext context = new ApplicationDbContext();
				var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
				var s = UserManager.GetRoles(user.GetUserId());
				if (s[0].ToString() == "Admin")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}
		
		//----------------------------------------------------------------------------------------------------\\
		//add errors
		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}		

		//----------------------------------------------------------------------------------------------------\\
	}
}
//---------------------------------------------------------End of File----------------------------------------------------\\