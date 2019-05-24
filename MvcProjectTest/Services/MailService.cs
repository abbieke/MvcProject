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
            //var apiKey = ConfigurationManager.AppSettings["SendGrid_1G"];
            //var client = new SendGridClient(apiKey);
            //var from = new EmailAddress("Hello1G", "1G Chou");
            //var subject = "Sending with SendGrid is Fun";
            //var to = new EmailAddress("jay5eternal@gmail.com", "Example User");
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg);
            //await Task.FromResult(0);
            var apiKey = ConfigurationManager.AppSettings["SendGrid_1G"];
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Hi1GLOL@gmail.com", "1G Team"),
                Subject = "Hello World from the SendGrid CSharp SDK!666",
                PlainTextContent = "Hello, Email bobo!", 
                HtmlContent = "<p>感謝註冊會員,為了....點擊下方按鈕驗證</p><a href='http://localhost:49572/Account/Test2'>驗證</a>"
            };
            msg.AddTo(new EmailAddress("abbieke0222@gmail.com", "Test User"));
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
    }
}