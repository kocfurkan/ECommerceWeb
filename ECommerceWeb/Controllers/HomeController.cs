using ECommerceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceWeb.Controllers
{
    public class HomeController : Controller
    {
        private ECommerceEntities db = new ECommerceEntities();
        public ActionResult Index()
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Products = db.Products.ToList();

            return View();
        }
        public ActionResult Category(int id)
		{
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.CategoryName = db.Categories.FirstOrDefault(x=>x.CategoryId == id);
            return View(db.Products.Where(x=>x.CategoryId==id).ToList());
		}
        public ActionResult Product(int id)
        {
            ViewBag.Categories = db.Categories.ToList();
            return View(db.Products.Find(id));
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}