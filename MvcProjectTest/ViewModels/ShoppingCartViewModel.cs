using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcProjectTest.ViewModels
{
    public class ShoppingCartViewModel
    {
        public string BookID { get; set; }
        public int Quantity { get; set; }
        public int BooksNo { get; set; }
        public string BooksName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }
        public int InStock { get; set; }
        public decimal Discount { get; set; }
        public string BookImage { get; set; }
        public string CategoryEngName { get; set; }



    }
}