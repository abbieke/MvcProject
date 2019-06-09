using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcProjectTest.ViewModels;
using MvcProjectTest.Models;
using MvcProjectTest.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MvcProjectTest.Services
{
    public class OrderService
    {

        OrderRepository _orderRepo = new OrderRepository();


        public string GetFakeOrderNo()
        {
            string lastNo = _orderRepo.GetMaxNo();
            int orderNoLength = lastNo.Length;
            string front = lastNo.Substring(0, 2);

            string newNum = (int.Parse(lastNo.Substring(2)) + 1).ToString();

            return front + newNum.PadLeft(orderNoLength - 2, '0');
        }


        public enum PayWay
        {
            貨到付款, 信用卡, LinePay
        }
        public enum DeliveryMethod
        {
            宅配, 超商取貨
        }
    }
}