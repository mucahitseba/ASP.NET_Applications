using Admin.BLL.Identity;
using Admin.BLL.Services;
using Admin.MODELS.IdentityModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Admin.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            new BarcodeService().Get("8690698032203");
            var roller = new string[] { "Admin", "User" };
            var roleManager = MembershipTools.NewRoleManager();
            foreach (var rol in roller)
            {
                if (!roleManager.RoleExists(rol))
                {
                    roleManager.Create(new Role()
                    {
                        Name = rol
                    });
                }
            }
        }
    }
}
