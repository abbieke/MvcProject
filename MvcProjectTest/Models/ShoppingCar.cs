using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProjectTest.Models
{
    public class ShoppingCar
    {
        public int ShoppingCarID { get; set; }
        public int CustomerId { get; set; }
        public string BookID { get; set; }
        public int Quantity { get; set; }

    }
}