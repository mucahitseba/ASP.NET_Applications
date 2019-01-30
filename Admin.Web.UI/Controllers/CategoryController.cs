using Admin.BLL.Repository;
using Admin.MODELS.Entities;
using System;
using System.Collections.Generic;
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
        public ActionResult Add(Category model)
        {
            ViewBag.CategoryList = GetCategorySelectList();
            try
            {
                model.TaxRate /= 100;
                if (model.SupCategoryId == 0) model.SupCategoryId = null;
                new CategoryRepo().Insert(model);
                return RedirectToAction("Add");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Add");
            }
        }
    }
}