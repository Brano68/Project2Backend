using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarRental.Email
{
    public class SendEmail
    {
        

        public  void sendEmailToCustomer(string adress)
        {
            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("carrentalke@gmail.com"));
            email.To.Add(MailboxAddress.Parse(adress));
            email.Subject = "CarReservation";
            email.Body = new TextPart(TextFormat.Plain) { Text = "Your reservation has been completed!!!" };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("carrentalke@gmail.com", "Kosice2021");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
