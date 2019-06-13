using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProjectTest.Models
{
    public class MixModel
    {
    }
    public class Mix
    {
        public int Id { get; set; }
        public List<Book> Books { get; set; }
        public List<Book> SecBooks { get; set; }
        public List<Book> ThirdBooks { get; set; }
        public Author TopAuthor { get; set; }
    }

    public class BookMix
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public List<Book> SelectBooks { get; set; }
    }

    public class CustomerMix
    {
        public CustomerViewModel CustomerView { get; set; }
        public CustomerChangePasswordViewModel ChangePassword { get; set; }
        public List<Order> Order { get; set; }
    }

    //public class OrderMix
    //{
    //    public List<Order> Order { get; set; }
    //    public List<OrderStatusModel> OrderStatus { get; set; }
    //}

    public class OrderDetailMix
    {
        public Order Order { get; set; }
        public List<OrderDetailModel> OrderDetails { get; set; }
        public OrderStatusModel OrderStatus { get; set; }
    }

    public class BookTypeMix
    {
        public List<Book> Books { get; set; }
        public List<BookType> BookTypes { get; set; }
    }

    public class CustomerDetail
    {
        public Customer Customer { get; set; }
        public CustomerOrderCount OrderCount { get; set; }
    }
}