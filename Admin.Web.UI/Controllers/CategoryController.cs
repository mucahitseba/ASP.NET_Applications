using Admin.BLL.Helpers;
using Admin.BLL.Repository;
using Admin.MODELS.Entities;
using Admin.MODELS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class CategoryController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.CategoryList = GetCategorySelectList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            try
            {
                if (model.SupCategoryId == 0) model.SupCategoryId = null;
                if (!ModelState.IsValid) //veritabanıyla annotation validation işlemini doğrular
                {
                   // ModelState.AddModelError("CategoryName", "100 karakteri geçme");
                    model.SupCategoryId = model.SupCategoryId ?? 0;
                    ViewBag.CategoryList = GetCategorySelectList();
                    return View(model);
                }

                
                if (model.SupCategoryId > 0)
                {
                    model.TaxRate = new CategoryRepo().GetById(model.SupCategoryId).TaxRate;
                }

                new CategoryRepo().Insert(model);
                TempData["Message"] = $"{model.CategoryName} isimli kategori başarıyla eklendi";
                return RedirectToAction("Add");
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu{EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu{ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode=500
                };
                return RedirectToAction("Error", "Home");
            }

        }
    }
}