using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProjectTest.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string PayWay { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public string Recipient { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientAddress { get; set; }
        public decimal ShippingRate { get; set; }

        public DateTime? SetUp { get; set; }
        public DateTime? Preparation { get; set; }
        public DateTime? Delivery { get; set; }
        public DateTime? PickUp { get; set; }
        public DateTime? CompletePickup { get; set; }
        public DateTime? TransactionComplete { get; set; }
    }

    public class OrderDetailModel
    {
        public int OrderID { get; set; }
        public string BookID { get; set; }
        public string BooksName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Counts { get; set; }
        public decimal Discount { get; set; }
    }

    public class OrderStatusModel
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime? SetUp { get; set; }
        public DateTime? Preparation { get; set; }
        public DateTime? Delivery { get; set; }
        public DateTime? PickUp { get; set; }
        public DateTime? CompletePickup { get; set; }
        public DateTime? TransactionComplete { get; set; }
    }

    public class CustomerOrderCount
    {
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}