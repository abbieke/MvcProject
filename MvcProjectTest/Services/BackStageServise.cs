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
        public IEnumerable<Customer> GetMember()
        {
            var result = _cusRepo.ReadAllCustomer();
            return result;
        }

        public IEnumerable<Order> GetOrderDetail()
        {
            var result =_orderRepo.GetOrderDetail();
            return result;
        }

    }
}