using ASP.NET.Models;
using ASP.NET.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllCategories()
        {
            try
            {
                var db = new NorthwindEntities();
                var data = db.Categories.Select(x => new CategoryViewModel()
                {
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                    CategoryID = x.CategoryID,
                    ProductCount = x.Products.Count
                }).ToList();
                return Json(new ResponseData()
                {
                    success = true,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    success = false,
                    message = $"Bir hata olustu {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetAllProducts()
        {
            try
            {
                var db = new NorthwindEntities();
                var data = db.Products.OrderBy(x => x.ProductID)
                    .ToList()
                    .Select(x => new ProductViewModel()
                    {
                        CategoryName = x.Category?.CategoryName,
                        CategoryID=x.CategoryID,
                        ProductName = x.ProductName,
                        UnitsInStock = x.UnitsInStock,
                        UnitPrice = x.UnitPrice,
                        ProductID = x.ProductID,
                        Discontinued = x.Discontinued,
                        QuantityPerUnit = x.QuantityPerUnit,
                        ReorderLevel = x.ReorderLevel,
                        SupplierID = x.SupplierID,
                        SupplierName = x.Supplier?.CompanyName,
                        UnitPriceFormatted = $"{x.UnitPrice:c2}",
                        UnitsOnOrder = x.UnitsOnOrder
                    })
                    .ToList();
                return Json(new ResponseData()
                {
                    success = true,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    success = false,
                    message = $"Bir hata olustu {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Search(string s)
        {
            var key = s.ToLower();
            if (key.Length <= 2 && key != "*")
            {
                return Json(new ResponseData()
                {
                    message = "Aramak icin 2 karakterden fazlasini girin",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var db = new NorthwindEntities();
                List<ProductViewModel> data;
                if (key == "*")
                {
                    data = db.Products.OrderBy(x => x.ProductID)
                        .Select(x => new ProductViewModel()
                        {
                            ProductName = x.ProductName,
                            SupplierID = x.SupplierID,
                            ProductID = x.ProductID,
                            CategoryID = x.CategoryID,
                            QuantityPerUnit = x.QuantityPerUnit,
                            UnitPrice = x.UnitPrice,
                            UnitsInStock = x.UnitsInStock,
                            UnitsOnOrder = x.UnitsOnOrder,
                            ReorderLevel = x.ReorderLevel,
                            Discontinued = x.Discontinued
                            
                        }).ToList();
                }
                else
                {
                    data = db.Products
                        .Where(x =>
                            x.ProductName.ToLower().Contains(key)
                            )
                        .Select(x => new ProductViewModel()
                        {
                            ProductName = x.ProductName,
                            SupplierID = x.SupplierID,
                            ProductID = x.ProductID,
                            CategoryID = x.CategoryID,
                            QuantityPerUnit = x.QuantityPerUnit,
                            UnitPrice = x.UnitPrice,
                            UnitsInStock = x.UnitsInStock,
                            UnitsOnOrder = x.UnitsOnOrder,
                            ReorderLevel = x.ReorderLevel,
                            Discontinued = x.Discontinued
                            
                        })
                        .ToList();
                }
                return Json(new ResponseData()
                {
                    message = $"{data.Count} adet kayit bulundu",
                    success = true,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata olustu {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CategorySearch(int? id)
        {
            
            try
            {
                var db = new NorthwindEntities();
                List<ProductViewModel> data;
                if (id != null)
                {
                    data = db.Products.OrderBy(x => x.ProductName).ToList().Where(x=>x.CategoryID==id.Value)
                        .Select(x => new ProductViewModel()
                        {
                            CategoryName=x.Category?.CategoryName,
                            ProductName = x.ProductName,
                            SupplierID = x.SupplierID,
                            ProductID = x.ProductID,
                            CategoryID = x.CategoryID,
                            QuantityPerUnit = x.QuantityPerUnit,
                            UnitPrice = x.UnitPrice,
                            UnitsInStock = x.UnitsInStock,
                            UnitsOnOrder = x.UnitsOnOrder,
                            ReorderLevel = x.ReorderLevel,
                            Discontinued = x.Discontinued

                        }).ToList();
                    return Json(new ResponseData()
                    {
                        success = true,
                        data = data
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ResponseData()
                    {
                        message = "Kategori Seciniz",
                        success = false,

                    }, JsonRequestBehavior.AllowGet);
                }              
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata olustu {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Add(ProductViewModel model)
        {
            try
            {
                var db = new NorthwindEntities();
                db.Products.Add(new Product()
                {
                    ProductName = model.ProductName,
                    SupplierID = model.SupplierID,
                    CategoryID = model.CategoryID,
                    QuantityPerUnit = model.QuantityPerUnit,
                    UnitPrice = model.UnitPrice,
                    UnitsInStock = model.UnitsInStock,
                    UnitsOnOrder = model.UnitsOnOrder,
                    ReorderLevel = model.ReorderLevel,
                    Discontinued = model.Discontinued

                });
                db.SaveChanges();
                return Json(new ResponseData()
                {
                    message = $"{model.ProductName} ismindeki ürün basariyla eklendi",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata olustu {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var db = new NorthwindEntities();
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return Json(new ResponseData()
                {
                    message = $"{product.ProductName} ismindeki ürün basariyla silindi",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Ürün silme isleminde hata {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Update(Product model)
        {
            try
            {
                var db = new NorthwindEntities();
                var product = db.Products.Find(model.ProductID);
                if (product == null)
                {
                    return Json(new ResponseData()
                    {
                        message = $"Ürün bulunamadi",
                        success = false
                    }, JsonRequestBehavior.AllowGet);
                }
                product.ProductName = model.ProductName;
                product.SupplierID = model.SupplierID;
                product.CategoryID = model.CategoryID;
                product.QuantityPerUnit = model.QuantityPerUnit;
                product.UnitPrice = model.UnitPrice;
                product.UnitsInStock = model.UnitsInStock;
                product.UnitsOnOrder = model.UnitsOnOrder;
                product.ReorderLevel = model.ReorderLevel;
                product.Discontinued = model.Discontinued;
                db.SaveChanges();
                return Json(new ResponseData()
                {
                    message = $"{product.ProductName} ismindeki ürün basariyla guncellendi",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"ürün guncelleme isleminde hata {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}