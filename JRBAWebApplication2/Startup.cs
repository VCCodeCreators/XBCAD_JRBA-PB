using Microsoft.Owin;
using Owin;
using System.Web.Services.Description;

[assembly: OwinStartupAttribute(typeof(JRBAWebApplication2.Startup))]
namespace JRBAWebApplication2
{
	public partial class Startup
	{
        public void Configuration(IAppBuilder app )
        {
            ConfigureAuth(app);


        }



    }
}
//------------------------------------------------End of File----------------------------------------------------\\
