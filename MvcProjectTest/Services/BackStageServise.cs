using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcProjectTest.Repositories;
using MvcProjectTest.Services;
using MvcProjectTest.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MvcProjectTest.Models;

namespace MvcProjectTest.Services
{
    public class BackStageServise
    {
        private readonly CustomersRepository _cusRepo = new CustomersRepository();
        private readonly OrderRepository _orderRepo = new OrderRepository();
        private readonly BooksRepository _bookRepo = new BooksRepository();
        public IEnumerable<Customer> GetMember()
        {
            var result = _cusRepo.ReadAllCustomer();
            return result;
        }

        public IEnumerable<OrderDetailModel> GetOrderDetail()
        {
            var result =_orderRepo.GetOrderDetail();
            return result;
        }

        public IEnumerable<Book> OrderDetailToBooks(IEnumerable<OrderDetailModel> allDetail)
        {
            List<Book> result = new List<Book>();

            foreach (OrderDetailModel od in allDetail)
            {
                Book book = _bookRepo.SelectBook(od.BookID);
                result.Add(book);
            
            }
            return result;
        }

    }
}