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
                CategoryName= catrgory.Name
               
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
    }

    internal class CategoryMod
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
