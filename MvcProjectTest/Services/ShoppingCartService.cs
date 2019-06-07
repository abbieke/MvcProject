﻿using System;
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
        ShoppingRepository cartRepo = new ShoppingRepository();
        BooksRepository bookRepo = new BooksRepository();
        public IEnumerable<ShoppingCartViewModel> GetMemberCart(string account)
        {
            var model = new List<ShoppingCartViewModel>();
            

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

        public void DeleteCartByAccount(string account)
        {

            if(account != null)
            {
                cartRepo.DeleteByAccount(account);
            }
            else
            {
                throw new Exception("沒有成功取得用戶帳號名稱，請檢查");
            }
            
        }

        public static string GetErrorText(Error error)
        {
            switch (error)
            {
                case Error.accountError:
                    return "會員資料認證錯誤，請重試";
                case Error.inStockError:
                    return "商品庫存不足，請重試";
                case Error.otherError:
                    return "意外錯誤，請重試";
                
            }
            return "錯誤資訊未標明，請重試";
        }

        public enum Error
        {
            accountError, inStockError, otherError
        }


    }
}