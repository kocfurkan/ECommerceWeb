using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ECommerceWeb.Models;
using Newtonsoft.Json;

namespace ECommerceWeb.Controllers
{
    public class CategoriesController : Controller
    {
        private ECommerceEntities db = new ECommerceEntities();
        //Used for connecting API
        HttpClient client = new HttpClient();
        // GET: Categories
        public ActionResult Index()
        {
            List<Categories> categories = new List<Categories>();
            client.BaseAddress = new Uri("https://localhost:44321/api/");
            var response =client.GetAsync("Category");  //Add to uri for reaching controller action.
            response.Wait(); //Wait for response
            var result = response.Result;  //Save the response
            if(result.IsSuccessStatusCode)  //If Response Code Is OK
            {
                var data = result.Content.ReadAsStringAsync();  //Read Json as String
                data.Wait();
                categories = JsonConvert.DeserializeObject<List<Categories>>(data.Result); //Convert it to required data type
            }
            return View(categories);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Async Post Through API
                client.BaseAddress = new Uri("https://localhost:44321/api/");
                var response = HttpClientExtensions.PostAsJsonAsync<Categories>(client, "Category", categories);
                response.Wait();
                var result = response.Result;
                if(result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                //db.Categories.Add(categories);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categories);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Categories/Edit/5
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Categories categories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = db.Categories.Find(id);
            db.Categories.Remove(categories);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
