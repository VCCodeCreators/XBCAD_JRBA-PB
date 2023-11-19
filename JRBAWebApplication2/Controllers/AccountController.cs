using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using JRBAWebApplication2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Entity.Validation;

namespace JRBAWebApplication2.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		//Property Decloration\\
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private readonly UserManager<ApplicationUser> userManager;

		private readonly ApplicationDbContext db = new ApplicationDbContext();
		readonly ApplicationDbContext context;

		public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			this.userManager = userManager;
			this.context = context;

		}
		//----------------------------------------------------------------------------------------------------\\
		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}
		//----------------------------------------------------------------------------------------------------\\
		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
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
		/// <summary>
		/// GET: /Account/Login
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// POST: /Account/Login
		/// </summary>
		/// <param name="model"></param>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectUser(returnUrl);
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid login attempt.");
					return View(model);
			}
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// redirects the user
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		private ActionResult RedirectUser(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Dashboard", "Home");
			}
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/VerifyCode
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="returnUrl"></param>
		/// <param name="rememberMe"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
		{
			// Require that the user has already logged in via username/password or external login
			if (!await SignInManager.HasBeenVerifiedAsync())
			{
				return View("Error");
			}
			return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// POST: /Account/VerifyCode
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// The following code protects for brute force attacks against the two factor codes. 
			// If a user enters incorrect codes for a specified amount of time then the user account 
			// will be locked out for a specified amount of time. 
			// You can configure the account lockout settings in IdentityConfig
			var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(model.ReturnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid code.");
					return View(model);
			}
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/Register
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult Register()
		{

			return View();
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// POST: /Account/Register
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			if (ModelState.IsValid)
			{
				try
				{
					var existingUser = await UserManager.FindByEmailAsync(model.Email);
					if (existingUser != null)
					{
						ModelState.AddModelError("Email", "Email not found or matched");
						return View(model);
					}

					var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
					var result = await UserManager.CreateAsync(user, model.Password);

					if (result.Succeeded)
					{
						if (!await RoleManager.RoleExistsAsync("Common"))
						{
							await RoleManager.CreateAsync(new IdentityRole("Common"));
						}

						model.UserRoles = "Common";
						await UserManager.AddToRoleAsync(user.Id, model.UserRoles);
						await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

						using (var dbContext = new ApplicationDbContext())
						{
							var userData = new UserModel
							{
								UserName = model.Email,
								UserId = user.Id,
								Email = model.Email,
								UserRoles = model.UserRoles,
								FirstName = model.FirstName,
								LastName = model.LastName,
								Password = model.Password
							};

							//dbContext.Users.Add(userData);
							dbContext.SaveChanges(); // Save changes to the database
						}

						return RedirectToAction("Index", "Home");
					}
					else
					{
						AddErrors(result);
					}
				}
				catch (DbEntityValidationException ex)
				{
					// Handle validation errors, as you did
				}
			}

			return View(model);
		}


		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/ConfirmEmail
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return View("Error");
			}
			var result = await UserManager.ConfirmEmailAsync(userId, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/ForgotPassword
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View();
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// POST: /Account/ForgotPassword
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindByNameAsync(model.Email);
				if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
				{
					// Don't reveal that the user does not exist or is not confirmed
					return View("ForgotPasswordConfirmation");
				}

				// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
				// Send an email with this link
				// string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
				// var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
				// await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
				// return RedirectToAction("ForgotPasswordConfirmation", "Account");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/ForgotPasswordConfirmation
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/ResetPassword
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult ResetPassword(string code)
		{
			return code == null ? View("Error") : View();
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// POST: /Account/ResetPassword
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await UserManager.FindByNameAsync(model.Email);
			if (user == null)
			{
				// Don't reveal that the user does not exist
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
			if (result.Succeeded)
			{
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			AddErrors(result);
			return View();
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/ResetPasswordConfirmation
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// POST: /Account/ExternalLogin
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/SendCode
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <param name="rememberMe"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
		{
			var userId = await SignInManager.GetVerifiedUserIdAsync();
			if (userId == null)
			{
				return View("Error");
			}
			var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
			var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
			return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// POST: /Account/SendCode
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SendCode(SendCodeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			// Generate the token and send it
			if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
			{
				return View("Error");
			}
			return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/ExternalLoginCallback
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
				case SignInStatus.Failure:
				default:
					// If the user does not have an account, then prompt the user to create an account
					ViewBag.ReturnUrl = returnUrl;
					ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
					return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
			}
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// POST: /Account/ExternalLoginConfirmation
		/// </summary>
		/// <param name="model"></param>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Manage");
			}

			if (ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await AuthenticationManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					return View("ExternalLoginFailure");
				}
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
				var result = await UserManager.CreateAsync(user);
				if (result.Succeeded)
				{
					result = await UserManager.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded)
					{
						await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
						return RedirectToLocal(returnUrl);
					}
				}
				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// POST: /Account/LogOff
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// GET: /Account/ExternalLoginFailure
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}
		//----------------------------------------------------------------------------------------------------\\
		/// <summary>
		/// dispose  function
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_userManager != null)
				{
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null)
				{
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}
		//----------------------------------------------------------------------------------------------------\\

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}

}
//------------------------------------------------End of File----------------------------------------------------\\
