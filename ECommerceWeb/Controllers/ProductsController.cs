using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ECommerceWeb.Models;
using Newtonsoft.Json;

namespace ECommerceWeb.Controllers
{
    public class ProductsController : Controller
    {
        private ECommerceEntities db = new ECommerceEntities();
        HttpClient client = new HttpClient();
        public Task<HttpResponseMessage> response;
        public HttpResponseMessage result;
        // GET: Products
        public ActionResult Index()
        {
            List<Products> products = new List<Products>();
            client.BaseAddress = new Uri("https://localhost:44321/api/"); //add base address to client
            response =client.GetAsync("Product");  //Add base uri for reaching controller action.
            response.Wait(); //Wait for response
            result = response.Result;  //Save the response
            if(result.IsSuccessStatusCode)  //If Response Code Is OK
            {
                var data = result.Content.ReadAsStringAsync();  //Read Json as String
                data.Wait();
                products = JsonConvert.DeserializeObject<List<Products>>(data.Result); //Convert it to required data type
            }
            for (int i = 0; i < products.Count; i++)
            {
                products[i].Categories= db.Categories.Find(products[i].CategoryId);
            }
            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = FindProduct(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products, HttpPostedFileBase productImage)
        {
            if (ModelState.IsValid)
            {
                client.BaseAddress = new Uri("https://localhost:44321/api/");
                response = HttpClientExtensions.PostAsJsonAsync<Products>(client, "Product", products);
                response.Wait();
                result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    if (productImage != null)
                    {
                        var data = result.Content.ReadAsAsync<Products>();
                        data.Wait();
                        Products products1 = data.Result;
                        string file = Path.Combine(Server.MapPath("~/Images/"), products1.ProductId + ".jpg");
                        productImage.SaveAs(file);
                    }
                    return RedirectToAction("Index");

                }
               
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", products.CategoryId);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = FindProduct(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", products.CategoryId);
            return View(products);
        }

        // POST: Products/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products, HttpPostedFileBase productImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
				if (productImage != null)
				{
					string file = Path.Combine(Server.MapPath("~/Images/"), products.ProductId + ".jpg");
                    //    FileInfo fileInfo = new FileInfo(file);
                    //    fileInfo.Delete();
                    productImage.SaveAs(file);
                }
				//string file2 = Path.Combine(Server.MapPath("~/Images/"), products.ProductId + ".jpg");
				
				return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", products.CategoryId);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
        
            string file = Path.Combine(Server.MapPath("~/Images/"), id+ ".jpg");
            FileInfo fileInfo = new FileInfo(file);
            if(fileInfo.Exists)
            {
                fileInfo.Delete();
            }
               
           
            return RedirectToAction("Index");
        }

        private Products FindProduct(int? id)
        {
            Products product = null;
            client.BaseAddress = new Uri("https://localhost:44321/api/");
            response = client.GetAsync("Product/" + id.ToString());
            response.Wait();
            result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsAsync<Products>();
                data.Wait();
                product = data.Result;
            }

            return product;
        }


    }
}
