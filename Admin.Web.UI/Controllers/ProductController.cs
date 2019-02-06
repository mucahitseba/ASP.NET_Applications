using Admin.BLL.Helpers;
using Admin.BLL.Repository;
using Admin.BLL.Services;
using Admin.MODELS.Entities;
using Admin.MODELS.Models;
using Admin.MODELS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        
        public ActionResult Add()
        {
            ViewBag.ProductList = GetProductSelectList();
            ViewBag.CategoryList = GetCategorySelectList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Add(Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductList = GetProductSelectList();
                ViewBag.CategoryList = GetCategorySelectList();
                return View(model);
            }

            try
            {
                if (model.SupProductId.ToString().Replace("0", "").Replace("-", "").Length == 0)
                    model.SupProductId = null;
                model.LastPriceUpdateDate = DateTime.Now;
                await new ProductRepo().InsertAsync(model);
                TempData["Message"] = $"{model.ProductName} isimli ürün başarıyla eklenmiştir";
                return RedirectToAction("Add");
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Add",
                    ControllerName = "Product",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Product",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public ActionResult Update(Guid id)
        {
            ViewBag.ProductList = GetProductSelectList();
            ViewBag.CategoryList = GetCategorySelectList();
            var data = new ProductRepo().GetById(id);
            if (data == null)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Ürün bulunamadı",
                    ActionName = "Add",
                    ControllerName = "Product",
                    ErrorCode = 404
                };
                return RedirectToAction("Error", "Home");
            }
            return View(data);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Update(Product model)
        {
            try
            {
                // if (model.SupProductId !=null) model.SupProductId = null;
                if (!ModelState.IsValid)
                {
                    model.SupProductId = model.SupProductId ?? null;
                    ViewBag.ProductList = GetProductSelectList();
                    ViewBag.CategoryList = GetCategorySelectList();
                    return View(model);
                }
                if (model.SupProductId != null)
                {
                    model.CategoryId = new ProductRepo().GetById(model.SupProductId).CategoryId;
                }
                var data = new ProductRepo().GetById(model.Id);
                data.ProductName = model.ProductName;
                data.Barcode = model.Barcode;
                data.BuyPrice = model.BuyPrice;
                data.CategoryId = model.CategoryId;
                data.Description = model.Description;
                data.ProductType = model.ProductType;
                data.Quantity = model.Quantity;
                data.SalesPrice = model.SalesPrice;
                data.SupProductId = model.SupProductId;

                new ProductRepo().Update(data);
                foreach (var dataProduct in data.Products)
                {
                    dataProduct.CategoryId = data.CategoryId;
                    new ProductRepo().Update(dataProduct);
                    if (dataProduct.Products.Any()) UpdateSubCategoryId(dataProduct.Products);
                }
                void UpdateSubCategoryId(ICollection<Product> dataC)
                {
                    foreach (var dataCategory in dataC)
                    {
                        dataCategory.CategoryId = data.CategoryId;
                        new ProductRepo().Update(dataCategory);
                        if (dataCategory.Products.Any()) UpdateSubCategoryId(dataCategory.Products);
                    }
                }
                TempData["Message"] = $"{model.ProductName} isimli kategori başarıyla güncellenmiştir";
                ViewBag.ProductList = GetProductSelectList();
                ViewBag.CategoryList = GetCategorySelectList();
                return View(data);
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Add",
                    ControllerName = "Product",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Product",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public JsonResult CheckBarcode(string barcode)
        {
            try
            {
                if (new ProductRepo().Queryable().Any(x => x.Barcode == barcode))
                {
                    return Json(new ResponseData()
                    {
                        message = $"{barcode} sistemde kayıtlı",
                        success = true
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new ResponseData()
                {
                    message = $"{barcode} bilgisi servisten getirildi",
                    success = true,
                    data = new BarcodeService().Get(barcode)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu: {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
