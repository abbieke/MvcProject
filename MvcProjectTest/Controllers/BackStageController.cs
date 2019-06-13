using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcProjectTest.Repositories;
using MvcProjectTest.Services;
using MvcProjectTest.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MvcProjectTest.Models;
using System.IO;
using System.Web.UI;

namespace MvcProjectTest.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BackStageController : Controller
    {
        private readonly CustomersRepository _cusRepo;
        private readonly OrderRepository _orderRepo;
        private readonly BooksRepository _bookRepo;
        public BackStageController()
        {
            _cusRepo = new CustomersRepository();
            _orderRepo = new OrderRepository();
            _bookRepo = new BooksRepository();
        }
        // GET: BackStage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomerDetails(string id)
        {
            var CustDetail = _cusRepo.SelectCustomerDetail(id);
            var CustCount = _cusRepo.SelectCustomerOrderCount(id);
            CustomerDetail mix = new CustomerDetail(){ Customer = CustDetail, OrderCount = CustCount};
            return View(mix);
        }

        public ActionResult CustomerEdit(string id)
        {
            var CustDetail = _cusRepo.SelectCustomerView(id);
            int cust_id = _cusRepo.GetCusromerID(id);
            List<string> cust_roles = _cusRepo.SelectRoles(cust_id);

            ViewBag.isAdmin = cust_roles.Contains("2");
            return View(CustDetail);
        }

        public ActionResult CustomerIndex()
        {
            var Customers = _cusRepo.ReadAllCustomer();
            List<Customer> cust = new List<Customer>();
            cust = Customers.ToList();
            return View(cust);
        }

        public ActionResult CustomerUpdate(CustomerViewModel cust)
        {
            var current_cust = _cusRepo.SelectCustomerDetail(cust.CustomerAccount);
            if (current_cust.EmailConfirmed != cust.EmailConfirmed)
            {
                if (cust.EmailConfirmed)
                {
                    _cusRepo.CustomerRemoveRole(cust.CustomerId,"4");
                }
                else
                {
                    List<string> current_roles = _cusRepo.SelectRoles(cust.CustomerId);
                    current_roles.Add("4");
                    string str_current_roles = String.Join(",", current_roles.ToArray());
                    UserRoles userRoles = new UserRoles
                    {

                        UserID = cust.CustomerId,
                        RolesID = str_current_roles
                    };
                    _cusRepo.CustomerUpdateRole(userRoles);
                }

            }
            _cusRepo.UpdateCustomer(cust);
            
            return RedirectToAction("CustomerIndex", "BackStage");
        }
        public ActionResult UpdateAdminRole(string custid, bool isAdmin)
        {
            int int_custid = Convert.ToInt32(custid);
            List<string> current_roles = _cusRepo.SelectRoles(int_custid);
            if (current_roles.Contains("2") != isAdmin)
            {
                if (isAdmin)
                {
                    current_roles.Add("2");
                    string str_current_roles = String.Join(",", current_roles.ToArray());
                    UserRoles userRoles = new UserRoles
                    {

                        UserID = int_custid,
                        RolesID = str_current_roles
                    };
                    _cusRepo.CustomerUpdateRole(userRoles);

                }
                else
                {
                    _cusRepo.CustomerRemoveRole(int_custid, "2");
                }

            }

            return RedirectToAction("CustomerIndex", "BackStage");
        }

        public ActionResult OrderDetails(int? id)
        {
            var order = _cusRepo.SelectOrder(id);
            var orderDetail = _cusRepo.SelectOrderDetails(id);
            var orderStatus = _cusRepo.SelectOrderStatus(id);

            OrderDetailMix mix = new OrderDetailMix() { Order = order, OrderDetails = orderDetail, OrderStatus = orderStatus };
            return View(mix);
        }

        public ActionResult OrderEdit(string id)
        {
            var orderStatus = _orderRepo.GetOrderStatus(id);
            ViewBag.setUp = (!(orderStatus.SetUp == null));
            ViewBag.preparation = (!(orderStatus.Preparation == null));
            ViewBag.delivery = (!(orderStatus.Delivery == null));
            ViewBag.pickUp = (!(orderStatus.PickUp == null));
            ViewBag.completePickup = (!(orderStatus.CompletePickup == null));
            ViewBag.transactionComplete = (!(orderStatus.TransactionComplete == null));
            return View(orderStatus);
        }

        public ActionResult OrderUpdate(string OrderID, bool SetUpname, bool Preparationname, bool Deliveryname, bool PickUpname, bool CompletePickupname, bool TransactionCompletename)
        {
            OrderStatusModel orderStatusView = new OrderStatusModel();
            var orderStatus = _orderRepo.GetOrderStatus(OrderID);
            orderStatusView = orderStatus;
            if ((!(orderStatus.SetUp == null)) != SetUpname)
            {
                orderStatusView.SetUp = DateTime.Now;
            }
            if ((!(orderStatus.Preparation == null)) != Preparationname)
            {
                orderStatusView.Preparation = DateTime.Now;
            }
            if ((!(orderStatus.Delivery == null)) != Deliveryname)
            {
                orderStatusView.Delivery = DateTime.Now;
            }
            if ((!(orderStatus.PickUp == null)) != PickUpname)
            {
                orderStatusView.PickUp = DateTime.Now;
            }
            if ((!(orderStatus.CompletePickup == null)) != CompletePickupname)
            {
                orderStatusView.CompletePickup = DateTime.Now;
            }
            if ((!(orderStatus.TransactionComplete == null)) != TransactionCompletename)
            {
                orderStatusView.TransactionComplete = DateTime.Now;
            }
            _orderRepo.OrderStatusUpdate(orderStatusView);
            return RedirectToAction("OrderIndex", "BackStage");
        }

        public ActionResult OrderIndex()
        {
            var getOrders = _orderRepo.GetAllOrders();
            List<Order> orders = new List<Order>();
            orders = getOrders.ToList();
            return View(orders);
        }

        public ActionResult ProductCreate()
        {

            return View();
        }

        public ActionResult ProductDelete(string bookId)
        {
            Book model = _bookRepo.SelectBook(bookId);
            return View(model);
        }

        public ActionResult ProductEdit(string bookId)
        {
            Book model = _bookRepo.SelectBook(bookId);
            return View(model);
        }

        public ActionResult ProductDetails(string bookId)
        {
            Book model = _bookRepo.SelectBook(bookId);
            return View(model);
        }

        public ActionResult ProductIndex()
        {
            var model = _bookRepo.GetAllBook();
            return View(model);
        }

        public bool CreatePress(string pressName, string pressPhone, string pressAddress )
        {
            Press newPress = new Press();
            newPress.PressName = pressName;
            newPress.PressPhone = pressPhone;
            newPress.PressAddress = pressAddress;

            try
            {
                _bookRepo.CreatePress(newPress);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public bool CreateCategory(string cateName, string cateEngName)
        {
            Category newCate = new Category();
            newCate.CategoryName = cateName;
            newCate.CategoryEngName = cateEngName;

            try
            {
                _bookRepo.CreateCategory(newCate);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool CreateAuthor(string authorName)
        {
            RealAuthor newAuthor = new RealAuthor();
            newAuthor.AuthorName = authorName;

            try
            {
                _bookRepo.CreateRealAuthor(newAuthor);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool CreateBook(string bookId, string bookName, string pressName, string cateId, string authorName, string unitPrice, string inStock, string discount, string description, string ISBN, string bookImage)
        {
            Book book = new Book();
            book.BookId = bookId;
            book.BooksName = bookName;
            book.PressName = pressName;
            book.CategoryID = int.Parse( cateId);
            book.AuthorName = authorName;
            book.UnitPrice = int.Parse(unitPrice);
            book.InStock = int.Parse(inStock);
            book.Discount = decimal.Parse( discount);
            book.Description = description;
            book.ISBN = ISBN;
            book.BookImage = bookImage;



            try
            {
                _bookRepo.CreateBook(book);
                return true;
            }
            catch
            {
                return false;
            }

        }

        //[HttpPost]
        //public void UploadImg(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //        try
        //        {
        //            string path = Path.Combine(Server.MapPath("~/Assets/Images"),
        //                                       Path.GetFileName(file.FileName));
        //            file.SaveAs(path);
        //            ViewBag.Message = "File uploaded successfully";
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Message = "ERROR:" + ex.Message.ToString();
        //        }
        //    else
        //    {
        //        ViewBag.Message = "You have not specified a file.";
        //    }
            
        //}

        





    }
}