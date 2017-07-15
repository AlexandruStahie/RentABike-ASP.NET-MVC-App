using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BD_Project.Startup))]
namespace BD_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
        }

    }
}
