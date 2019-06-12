using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MvcProjectTest.Services
{
    public class MailService
    {
        public static async Task SendMailToVerify(string callbackUrl,string mail)
        {
            var apiKey = ConfigurationManager.AppSettings["SendGrid_1G"];
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Hi1GLOL@gmail.com", "1G Team"),
                Subject = "Hello World from the SendGrid CSharp SDK!666",
                PlainTextContent = "", 
                HtmlContent = "<p>感謝註冊會員,為了....點擊下方按鈕驗證</p><a href="+ callbackUrl + ">驗證</a>"
            };
            msg.AddTo(new EmailAddress(mail, null));
            var response = await client.SendEmailAsync(msg);

            //var msg = new SendGridMessage()
            //{
            //    From = new EmailAddress("Hello1G@service.com", "1G Chou"),
            //    Subject = "Hello World from the SendGrid CSharp SDK! 6666",
            //    PlainTextContent = "Hello, Email bobo!", 
            //    HtmlContent = "<p>感謝註冊會員,為了....點擊下方按鈕驗證</p><a href='http://localhost:49572/Account/Test2'>驗證</a>"
            //};
            //msg.AddTo(new EmailAddress("ruth265820@gmail.com", "Test User"));
            //var response = await client.SendEmailAsync(msg);
        }
        public static async Task SendMailToResetPwd(string callbackUrl, string mail)
        {
            var apiKey = ConfigurationManager.AppSettings["SendGrid_1G"];
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Hi1GLOL@gmail.com", "1G Team"),
                Subject = "重設密碼 - 新知書櫥",
                PlainTextContent = "",
                HtmlContent = "<p>信件內容........點擊連結重設密碼</p><a href=" + callbackUrl + ">點擊重設</a>"
            };
            msg.AddTo(new EmailAddress(mail, null));
            var response = await client.SendEmailAsync(msg);
        }

        public static async Task SendMailToNoticeOrderSuccess(string mail,string orderno)
        {
            var apiKey = ConfigurationManager.AppSettings["SendGrid_1G"];
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Hi1GLOL@gmail.com", "1G Team"),
                Subject =DateTime.Now.ToShortDateString()+ "訂單成立通知 - 新知書櫥",
                PlainTextContent = "",
                HtmlContent = "<p>你已成功在<a href='https://mvcprojecttest20190604123944.azurewebsites.net/'>新知書櫥網站</a>下訂單!</p>" +
                "<p>訂單編號為:"+ orderno + "</p>"+
                "<p>可以搭配 Line Bot(ID:@409hsreg)</p>"+
                "<p>或網站<a href='https://mvcprojecttest20190604123944.azurewebsites.net/Customer/CustomerIndex'>新知書櫥網站</a>查詢訂單進度喲!</p>" +
                "<p>謝謝您的下訂，歡迎再次光臨!</p>" +
                "</br>" +
                "<p>網站連結:<a href='https://mvcprojecttest20190604123944.azurewebsites.net/'>https://mvcprojecttest20190604123944.azurewebsites.net</a></p>" +
                "<p>Line Bot ID:@409hsreg (包含@)</p>"


            };
            msg.AddTo(new EmailAddress(mail, null));
            var response = await client.SendEmailAsync(msg);
        }
    }
}