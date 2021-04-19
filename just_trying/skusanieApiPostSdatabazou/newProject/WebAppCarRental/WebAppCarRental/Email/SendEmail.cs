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
        

        public  void sendEmailToCustomer()
        {
            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("carrentalke@gmail.com"));
            email.To.Add(MailboxAddress.Parse("branislav.nebus@kosickaakademia.sk"));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Plain) { Text = "Example Plain Text Message Body" };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("carrentalke@gmail.com", "Kosice2021");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
