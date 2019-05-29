using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjectTest.Controllers
{
    [Authorize]
    public class ShoppingController : Controller
    {
        // GET: Shopping
        public ActionResult Index()
        {
            return View(); 
        }
        public ActionResult ShippingInfo()
        {
            return View(); 
        }
        public ActionResult OrderCheck()
        {
            return View(); 
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }
        
    }
}