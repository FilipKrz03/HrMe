using Domain.Abstractions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace Application.Services
{
    public class MailSedningService : IMailSendingService
    {

        private readonly IConfiguration _configuration;

        public MailSedningService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void SendEmail(string sourceEmail, string subject, string body)
        {
            var login = _configuration["MailSender:Login"];
            var password = _configuration["MailSender:Password"];
            var senderEmail = _configuration["MailSender:Mail"];

            MailjetClient client =
                new MailjetClient(login, password);

            MailjetRequest request = new MailjetRequest { Resource = Send.Resource }
            .Property(Send.FromEmail, senderEmail)
            .Property(Send.FromName, "Hrme")
            .Property(Send.Subject, subject)
            .Property(Send.TextPart, body)
            .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", sourceEmail}
                 }
                });

            MailjetResponse response = await client.PostAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }

        }
    }
}
