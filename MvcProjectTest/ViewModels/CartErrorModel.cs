using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcProjectTest.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MvcProjectTest.ViewModels
{
    public class CartErrorModel
    {
        public bool IsError { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ShoppingCartService.Error ErrorType { get; set; }
        public string ErrorText { get; set; }

    }
}