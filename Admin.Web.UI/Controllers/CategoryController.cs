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
        public ActionResult Add()
        {
            ViewBag.CategoryList = GetCategorySelectList();
            return View();
        }
    }
}