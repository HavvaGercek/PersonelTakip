using Pt.Ent.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Pt.BL.Settings
{
    public class SiteSettings
    {
        public static string SiteMail = "platoakademi2017@gmail.com";
        public static string SiteMailPassword = "plt2016plt";
        public static string SiteMailSmtpHost = "smtp.gmail.com";
        public static int SiteMailSmtpPort = 587;
        public static bool SiteMailEnableSsl = true;

        public async static Task SendMail(MailModel model)//task olduğu için async await yaptık
        {
            using (var smtp = new SmtpClient())
            {
                var message = new MailMessage();
                message.To.Add(new MailAddress(model.To));
                message.From = new MailAddress(SiteMail);
                if (!string.IsNullOrEmpty(model.Cc))
                    message.CC.Add(new MailAddress(model.Cc));
                if (!string.IsNullOrEmpty(model.Bcc))
                    message.Bcc.Add(new MailAddress(model.Bcc));
                message.Subject = model.Subject;
                message.IsBodyHtml = true;
                message.Body = model.Message;
                var credential = new NetworkCredential//sitede hesap açma defaul cons
                {
                    UserName = SiteMail,
                    Password = SiteMailPassword
                };
                smtp.Credentials = credential;
                smtp.Host = SiteMailSmtpHost;
                smtp.Port = SiteMailSmtpPort;
                smtp.EnableSsl = SiteMailEnableSsl;
                await smtp.SendMailAsync(message);//send mail metodunu asenkron yaptık
            }

        }


    }
}