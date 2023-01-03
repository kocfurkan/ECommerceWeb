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
			return View();
		}
		public ActionResult AddToCart(int id, int quantity)
		{
			string userId = User.Identity.GetUserId();
			Carts cartItem = db.Carts.FirstOrDefault(x => x.ProductId == id && x.UserId == userId);
			Products products = new Products();
			if (cartItem == null)
			{
				Carts newCart = new Carts()
				{
					UserId = userId,
					ProductId = id,
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
			return RedirectToAction("Index");
		}
	}
}