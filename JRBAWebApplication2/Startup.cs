﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JRBAWebApplication2.Startup))]
namespace JRBAWebApplication2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }



    }
}
//------------------------------------------------End of File----------------------------------------------------\\
