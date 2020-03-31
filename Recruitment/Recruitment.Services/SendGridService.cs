using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Recruitment.Services
{
    public static class SendGridService
    {
        public static async Task SendEmail(string receiver, string offerName, string apikey, string fileLink = "")
        {
            var client = new SendGridClient(apikey);

            List<Attachment> attachments = new List<Attachment>();
            Attachment attachment = new Attachment();
            attachment.Filename = fileLink;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("mini@recruitment.com", "MiniRecruitment"),
                Subject = "New application has been created",
                PlainTextContent = $"New application for {offerName} has been added! Download file from this link",
                HtmlContent = $"<a href=\"{fileLink}\">Download CV here</a> "
            };
            msg.AddTo(new EmailAddress(receiver, "HR member"));
            
            var response = await client.SendEmailAsync(msg);
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Wrong api key");
            }
        }
    }
}