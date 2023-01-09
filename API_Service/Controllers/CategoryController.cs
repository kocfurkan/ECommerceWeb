using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_Service.Models;


namespace API_Service.Controllers
{
  
    public class CategoryController : ApiController
    {
        ECommerceEntities db = new ECommerceEntities();

        public IEnumerable<Categories> Get()
        {
            // Entity Framework creates instances of a dynamically generated derived types that acts as a proxy for the entities.
            // This creates performance problems and errors while converting to JSON. Disabling it solves the problem(if only one table is accessed)
            // More Info on ===> https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/advanced-entity-framework-scenarios-for-an-mvc-web-application#proxy-classes
            //https://learn.microsoft.com/en-us/ef/ef6/fundamentals/proxies

            db.Configuration.ProxyCreationEnabled=false;
            List<Categories> list = db.Categories.ToList();

            return list;
        }
       
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Categories catrgory = db.Categories.Find(id);
            CategoryMod cat = new CategoryMod()
            {
                CategoryId=catrgory.CategoryId,
                Name= catrgory.Name
               
            };
            return Ok(cat);
        }
        [HttpPost]
        //Json
        public IHttpActionResult Post([FromBody] Categories category)
        {

            db.Categories.Add(category);

            db.SaveChanges();

            return Ok();
        }
        [HttpPut]
        //Json
        public IHttpActionResult Put([FromBody] Categories category)
        {

            db.Entry(category).State =System.Data.Entity.EntityState.Modified;

            db.SaveChanges();

            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Categories category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return Ok();
        }
    }

    internal class CategoryMod
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
