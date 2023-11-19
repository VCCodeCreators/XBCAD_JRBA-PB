using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
	using JRBAWebApplication2.Models; // Replace with your actual namespaces
using JRBAWebApplication2.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Routing;

namespace JRBAWebApplication2
{
	
public class DependencyClass : DefaultControllerFactory
{
    protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
    {
        if (controllerType != null)
        {
            // Check for the specific controller type, and provide dependencies as needed
            if (controllerType == typeof(AccountController))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var dbContext = new ApplicationDbContext();

                return new AccountController(userManager, dbContext);
            }

            // Handle other controllers and dependencies similarly

            // Fallback to the default behavior
            return base.GetControllerInstance(requestContext, controllerType);
        }
        return null;
    }
}
   

public class CustomControllerActivator : IControllerActivator
{
    public IController Create(RequestContext requestContext, Type controllerType)
    {
        if (controllerType != null)
        {
            // Check for the specific controller type, and provide dependencies as needed
            if (controllerType == typeof(AccountController))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var dbContext = new ApplicationDbContext();

                return new AccountController(userManager, dbContext);
            }

            // Handle other controllers and dependencies similarly

            // Fallback to the default behavior
            return DependencyResolver.Current.GetService(controllerType) as IController;
        }

        return null;
    }

		
	}

}