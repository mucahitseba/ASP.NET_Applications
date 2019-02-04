﻿using Admin.BLL.Identity;
using Admin.MODELS.IdentityModels;
using Admin.MODELS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index",model);
            }
            try
            {
                var userStore = MembershipTools.NewUserStore();
            var userManager = MembershipTools.NewUserManager();
            
            var rm = model.RegisterViewModel;
            var user = await userManager.FindByNameAsync(rm.Username);
            if (user != null)
            {
                ModelState.AddModelError("UserName", "Bu kullanıcı adı daha önce alınmıştır");
                return View("Index", model);
            }
                var newUser = new User()
                {
                    UserName = rm.Username,
                    Email = rm.Email,
                    Name = rm.Name,
                    Surname = rm.Surname
                };
                var result = await userManager.CreateAsync(newUser, rm.Password);
                if (result.Succeeded)
                {
                    if (userStore.Users.Count() == 1)
                    {
                        await userManager.AddToRoleAsync(newUser.Id, "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(newUser.Id, "User");
                    }
                    //todo kullanıcıya mail gönderilsin
                }
                else
                {
                    var err = "";
                    foreach (var resultError in result.Errors)
                    {
                        err += resultError + " ";
                    }
                    ModelState.AddModelError("", err);
                    return View("Index", model);
                }
                TempData["Message"] = "Kaydınız alınmıştır";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu{ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
    }
}