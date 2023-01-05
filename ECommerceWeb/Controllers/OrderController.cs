using ECommerceWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceWeb.Controllers
{
    public class OrderController : Controller
    {
        private ECommerceEntities db = new ECommerceEntities();
        // GET: Order
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            return View(db.Carts.Where(x => x.UserId == userid).ToList());
        }
        public ActionResult OrderComplete()
        {
            //    ClientID: Bankadan alınan mağaza kodu
            //    Amount:Sepetteki ürünlerin toplam tutar
            //    Oid:SiparişID
            //    confirmURL:Ödeme başarılı olduğunda gelen verilerin gösterileceği url
            //    errorURL:Ödeme sırasında hata olduysa gelen hatanın gösterileceği url
            //    RDN:Hash karşılaştırılıması için kullanılan bilgi
            //        StoreKEy:Güvenlik anahtarı.Bankanın sanal pos sayfasından alınır.
            //        TransactionType:"Auth"
            //        Instalment:""
            //        HashStr:HashSet oluşturulurken bankanın istediği bilgiler birleştirilir.
            //        Hash:Farklı değerler oluşturulup birleştirilir.

            string UserId = User.Identity.GetUserId();

            List<Carts> cartItems = db.Carts.Where(x => x.UserId == UserId).ToList();

            string ClientId = "1003001";//Bankanın verdiği magaza kodu
            string TotalPrice = cartItems.Sum(x => x.TotalPrice).ToString();

            string sipId = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);

            string confirmURL = "https://localhost:44390/Order/Completed";

            string errorURL = "https://localhost:44390/Order/Error";

            string RDN = "asdf";
            string StoreKey = "123456";

            string TransActionType = "Auth";
            string Instalment = "";

            string HashStr = ClientId + sipId + TotalPrice + confirmURL + errorURL + TransActionType + Instalment + RDN + StoreKey;//Bankanın istediği bilgiler

            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            byte[] HashBytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(HashStr);
            byte[] InputBytes = sha.ComputeHash(HashBytes);
            string Hash = Convert.ToBase64String(InputBytes);

            ViewBag.ClientId = ClientId;
            ViewBag.Oid = sipId;
            ViewBag.okUrl = confirmURL;
            ViewBag.failUrl = errorURL;
            ViewBag.TransActionType = TransActionType;
            ViewBag.RDN = RDN;
            ViewBag.Hash = Hash;
            ViewBag.Amount = TotalPrice;
            ViewBag.StoreType = "3d_pay_hosting"; // Ödeme modelimiz
            ViewBag.Description = "";
            ViewBag.XID = "";
            ViewBag.Lang = "tr";
            ViewBag.EMail = "cenelif@gmail.com";
            ViewBag.UserId = "ElifCengiz"; // bu id yi bankanın sanala pos ekranında biz oluşturuyoruz.
            ViewBag.PostURL = "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate&quot";

            return View();
        }
        public ActionResult Completed()
        {
            string userId = User.Identity.GetUserId();
            Orders newOrder = new Orders()
            {
                Name = Request.Form.Get("Name"),
                Surname = Request.Form.Get("Surname"),
                Phone = Request.Form.Get("Phone"),
                Address = Request.Form.Get("Address"),
                IdNumber = Request.Form.Get("IdNumber"),
                Date = DateTime.Now,
                UserId = User.Identity.GetUserId()
            };
            List<Carts> currentCart = db.Carts.Where(x => x.UserId == userId).ToList();

            foreach (Carts item in currentCart)
            {
                OrderDetails od = new OrderDetails()
                {
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    TotalPrice = item.TotalPrice
                };
                newOrder.OrderDetails.Add(od);
                db.Carts.Remove(item);
            }
            db.Orders.Add(newOrder);
            db.SaveChanges();

            return View();
        }
        public ActionResult Error()
        {
            ViewBag.Error = Request.Form;
            return View();
        }
    }
}