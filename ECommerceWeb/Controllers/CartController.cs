using ECommerceWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceWeb.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private ECommerceEntities db = new ECommerceEntities();
		// GET: Cart
		public ActionResult Index()
		{
			string userid = User.Identity.GetUserId();
			return View(db.Carts.Where(x => x.UserId == userid).ToList());
		}
		public ActionResult AddToCart(int ProductId, int quantity)
		{
			string userId = User.Identity.GetUserId();
			Carts cartItem = db.Carts.FirstOrDefault(x => x.ProductId == ProductId && x.UserId == userId);
			Products products = db.Products.Find(ProductId);
			if (cartItem == null)
			{
				Carts newCart = new Carts()
				{
					UserId = userId,
					ProductId = ProductId,
					Quantity = quantity,
					TotalPrice = products.Price * quantity
				};
				db.Carts.Add(newCart);
			}
			else
			{
				cartItem.Quantity = cartItem.Quantity + quantity;
				cartItem.TotalPrice = cartItem.Quantity * products.Price;
			}
			  db.SaveChanges();
			return RedirectToAction("Index");
		}
		public ActionResult UpdateCart(int? cartid, int quantity)
        {
			if(cartid == null)
            {
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

			Carts myCart = db.Carts.Find(cartid);
			if(myCart == null)
            {
				return HttpNotFound();
            }
			Products currentProduct = db.Products.Find(myCart.ProductId);

			myCart.Quantity = quantity;
			myCart.TotalPrice = myCart.Quantity*currentProduct.Price;
			db.SaveChanges();
			return RedirectToAction("Index");
        }

		public ActionResult Delete(int? id)
        {
			if(id == null)
            {
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
			Carts myCart = db.Carts.Find(id);
			db.Carts.Remove(myCart);
			db.SaveChanges();
			return RedirectToAction("Index");

		}
	}
}