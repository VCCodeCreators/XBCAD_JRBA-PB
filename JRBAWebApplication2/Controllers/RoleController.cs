using JRBAWebApplication2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JRBAWebApplication2.Controllers
{
	public class RoleController : Controller
	{
		//Property Decloration\\
		ApplicationDbContext context;
		public RoleController()
		{
			context = new ApplicationDbContext();

		}
		UserController UC = new UserController();
		//----------------------------------------------------------------------------------------------------\\
		// GET: Role
		public ActionResult Index()
		{


			if (User.Identity.IsAuthenticated)
			{


				if (!UC.IsAdminUser())
				{
					return RedirectToAction("Index", "Home");
				}
			}
			else
			{
				return RedirectToAction("Dash", "Home");
			}
			//returns a list of user roles if the user is admin


			var Roles = context.Roles.ToList();
			return View(Roles);

		}
		//----------------------------------------------------------------------------------------------------\\
	}
}
//------------------------------------------------------End of File-------------------------------------------------------\\
