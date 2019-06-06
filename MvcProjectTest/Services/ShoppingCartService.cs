using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcProjectTest.ViewModels;
using MvcProjectTest.Models;
using MvcProjectTest.Repositories;

namespace MvcProjectTest.Services
{
    public class ShoppingCartService
    {
        public IEnumerable<ShoppingCartViewModel> GetMemberCart(string account)
        {
            var model = new List<ShoppingCartViewModel>();
            var cartRepo = new ShoppingRepository();
            var bookRepo = new BooksRepository();

            foreach(var item in cartRepo.GetMemberCart(account))
            {
                var book = bookRepo.GetBookById(item.BookID);
                var target = new ShoppingCartViewModel
                {
                    CartID = item.ShoppingCarID,
                    BookID = item.BookID,
                    BooksName = book.BooksName,
                    Discount = book.Discount,
                    InStock = book.InStock,
                    Quantity = item.Quantity,
                    BookImage = "Assets/Images/" + bookRepo.GetCategoryEngNameById(book.CategoryID) + "/" + book.BookImage,
                    UnitPrice = book.UnitPrice
                };
                model.Add(target);
            }
            
            return model;
        }

    }
}