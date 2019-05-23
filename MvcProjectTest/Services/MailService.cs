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
        public static async Task SendMailToVerify()
        {
            var apiKey = ConfigurationManager.AppSettings["SendGrid_1G"];
            var client = new SendGridClient(apiKey);
         
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Hi1G@service.com", "1G Team"),
                Subject = "Hello World from the SendGrid CSharp SDK!",
                //PlainTextContent = "Hello, Email bobo!", //純文字
                HtmlContent = "<p>感謝註冊會員,為了....點擊下方按鈕驗證</p><a href='http://localhost:49572/Account/Test2'>驗證</a>"
            };
            msg.AddTo(new EmailAddress("jay5eternal@gmail.com", "Test User"));
            var response = await client.SendEmailAsync(msg);
        }
    }
}