using MvcProjectTest.Models;
using MvcProjectTest.Repositories;
using MvcProjectTest.ViewModels;
using MvcProjectTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;

namespace MvcProjectTest.Controllers
{
    public class AccountController : Controller
    {
        public readonly CustomersRepository _repo;
        public AccountController()
        {
            _repo = new CustomersRepository();
        }

        public JsonResult CheckCustomerAccount(string CustomerAccount)
        {
            bool isValidate = false;
            //if (Url.IsLocalUrl(Request.Url.AbsoluteUri))
            //{
                //利用 IsLocalUrl檢查是否為網站呼叫的
                //借此忽略一些不必要的流量
                if (_repo.SelectCustomer(CustomerAccount))
                {
                    //因連資料庫麻煩
                    //所以假裝示範不可以註冊某一名字
                    isValidate = true;
                }
            //}
            // Remote 驗證是使用 Get 因此要開放
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckCustomerEmail(string CustomerEmail)
        {
            bool isValidate = false;
            //if (Url.IsLocalUrl(Request.Url.AbsoluteUri))
            //{
            //利用 IsLocalUrl檢查是否為網站呼叫的
            //借此忽略一些不必要的流量
            if (_repo.SelectCustomerEmail(CustomerEmail))
            {
                //因連資料庫麻煩
                //所以假裝示範不可以註冊某一名字
                isValidate = true;
            }
            //}
            // Remote 驗證是使用 Get 因此要開放
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Registration()
        {
            ViewData["Titlett"] = "會員註冊";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(Customer cust)
        {
            if(String.IsNullOrEmpty(cust.ConfirmPassword))
            {
                return Content("未確認密碼");
            }
            else
            {
                if (cust.CustomerAccount == cust.CustomerPassword)
                {
                    return Content("帳號密碼不可相同");
                }
                else
                {
                    if (cust.CustomerPassword == cust.ConfirmPassword)
                    {
                        Customer customer = new Customer
                        {
                            CustomerName = cust.CustomerName,
                            CustomerAddress = cust.CustomerAddress,
                            CustomerAccount = cust.CustomerAccount,
                            CustomerPassword = HashService.SHA256Hash(cust.CustomerPassword),
                            CustomerEmail = cust.CustomerEmail,
                            CustomerBirth = cust.CustomerBirth,
                            CustomerPhone = cust.CustomerPhone
                        };
                        _repo.InsertCustomer(customer);
                        var callbackUrl = Url.Action("EmailConfirmed", "Account",new { userAccount = cust.CustomerAccount },protocol: Request.Url.Scheme);
                        await MailService.SendMailToVerify(callbackUrl,cust.CustomerEmail);

                        return RedirectToAction("Index");
                    }
                    return Content("密碼不符");
                }
            }
        }
        
        public ActionResult EmailConfirmed(string userAccount)
        {
            int userId=_repo.GetCusromerID(userAccount);
            _repo.UpdateEmailConfirmed(userId, true);
            _repo.CustomerRemoveRole(userId, "4");

            return RedirectToAction("Index","Home",null);
        }

        //public ActionResult test()
        //{
        //    //return Content( _repo.CustomerRemoveRole(3, "4"));
        //}




        [HttpGet]
        public ActionResult Login()
        {
            ViewData["Titlett"] = "會員登入";
            //if(TempData["PageFrom"] is null)
            //{
            //    TempData["PageFrom"] = Url.Action("Index", "Home");
            //}
            ////TempData.Keep();
            //TempData["PageFrom"] = TempData["PageFrom"];
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginVM)
        {
            //未通過Model驗證
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            string account = HttpUtility.HtmlEncode(loginVM.CustomerAccount); //編碼成純文字
            string password = HttpUtility.HtmlEncode(loginVM.CustomerPassword);

            string sha256Password = HashService.SHA256Hash(password);
           
            //dapper
            Customer cust = _repo.CustomerLogin(account, sha256Password);
            if (cust == null)
            {
                ModelState.AddModelError("", "無效的帳號或密碼。");
                return View();
            }
            //FormsAuthentication Class -- https://docs.microsoft.com/zh-tw/dotnet/api/system.web.security.formsauthentication?view=netframework-4.8

            //FormsAuthenticationTicket Class
            //https://docs.microsoft.com/zh-tw/dotnet/api/system.web.security.formsauthenticationticket?view=netframework-4.8
            Session["userid"] = _repo.GetCusromerID(cust.CustomerAccount);
            string roles = GetRoles((int)Session["userid"]);
            //Create FormsAuthenticationTicket 以下
            var ticket = new FormsAuthenticationTicket(
            version: 1,
            name: cust.CustomerAccount.ToString(), //可以放使用者Id
            issueDate: DateTime.UtcNow,//現在UTC時間
            expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
            isPersistent: true,// 是否要記住我 true or false
            userData: roles, //可以放使用者角色名稱
            cookiePath: FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encryptedTicket = FormsAuthentication.Encrypt(ticket); //把驗證的表單加密

            // Create the cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
            

            // Redirect back to original URL.
            //var url = FormsAuthentication.GetRedirectUrl(account, true);

            //Response.Redirect(FormsAuthentication.GetRedirectUrl(name, true));
            //FormsAuthentication.GetRedirectUrl(account, true)


            //string pageFrom = TempData["PageFrom"].ToString();
            //TempData["PageFrom"] = null;
            
            return RedirectToAction("Index", "Home");
            //return Redirect(pageFrom);

            //return Content("Success!");
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();          
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {         
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ForgetPassword(ForgetPwdViewModel forgetPwdVM)
        {
            if (!ModelState.IsValid)
            {
                return View(forgetPwdVM);
            }

            string account = HttpUtility.HtmlEncode(forgetPwdVM.CustomerAccount); //編碼成純文字
            string birthday = HttpUtility.HtmlEncode(forgetPwdVM.CustomerBirth.ToString("yyyy/MM/dd"));
            //return Content(birthday);

            //dapper
            Customer cust = _repo.CustomerLogin(account);
            if (cust==null)
            {
                ModelState.AddModelError("", "無效的帳號。");
                return View();
            }
            if (cust.CustomerBirth.ToString("yyyy/MM/dd") == birthday)
            {
                ViewData["IsPass"] = true;
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userAccount = cust.CustomerAccount }, protocol: Request.Url.Scheme);
                await MailService.SendMailToResetPwd(callbackUrl, cust.CustomerEmail);

            }
            else
            {
                ModelState.AddModelError("", "會員生日不符。");
            }
            return View();
        }
        [HttpGet]
        public ActionResult ResetPassword(string userAccount)
        {
            Customer cust= _repo.CustomerLogin(userAccount);
            Session["userid"] = _repo.GetCusromerID(cust.CustomerAccount);
            string roles = GetRoles((int)Session["userid"]);
            var ticket = new FormsAuthenticationTicket(
            version: 1,
            name: cust.CustomerAccount.ToString(), //可以放使用者Id
            issueDate: DateTime.UtcNow,//現在UTC時間
            expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
            isPersistent: true,// 是否要記住我 true or false
            userData: roles, //可以放使用者角色名稱
            cookiePath: FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encryptedTicket = FormsAuthentication.Encrypt(ticket); //把驗證的表單加密

            // Create the cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPwdViewModel ResetPwdVM)
        {
            if (!ModelState.IsValid)
            {
                return View(ResetPwdVM);
            }
            _repo.UpdatePassword(User.Identity.Name, ResetPwdVM.ConfirmPassword);

            return RedirectToAction("Index", "Home");
        }
        public string GetRoles(int userid)
        {
            List<string> roles = _repo.SelectRoles(userid);
            List<string> names = new List<string>();
            foreach (string role in roles)
            {
                string rolename = _repo.SelectRolesName(role);
                names.Add(rolename);
            }
            string rolesEngNames = String.Join(",", names.ToArray());


            return rolesEngNames;
        }

        public async Task SendAsync(string account,string mail)
        {
            var callbackUrl = Url.Action("EmailConfirmed", "Account", new { userAccount =account }, protocol: Request.Url.Scheme);
            await MailService.SendMailToVerify(callbackUrl, mail);
        }
        

        
    }
}