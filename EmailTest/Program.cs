using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VtcIts;


namespace EmailTest
{
    class Program
    {
        static void Main(string[] args)
        {

            const string messageText = "test message";
            var message = new MailMessage
            {
                From = Settings.SystemAddress,
                Body = messageText,
                IsBodyHtml = true
            };
            message.Bcc.Add("rrush@kemtah.com");
            message.Bcc.Add("rush@rushputin.com");
            message.Bcc.Add("rrush@actionenergyhq.com");
            message.Bcc.Add("richard.rush@gmail.com");
            Mail.SendMail(message);
        }
    }
}
