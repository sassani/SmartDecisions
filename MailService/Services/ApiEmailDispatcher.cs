using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MailService.Services.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailService.Services
{
    public class ApiEmailDispatcher : IMessageDispatcher
    {
        private readonly MailServerSettings config;
        private static readonly HttpClient client = new HttpClient();
        public ApiEmailDispatcher(IOptions<MailServerSettings> options)
        {
            config = options.Value;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("api-key", config.ApiService.ApiKey);
            client.BaseAddress = new Uri(config.ApiService.Url);
        }

        public async Task DispatchMessageAsync(MimeMessage mMessage)
        {
            //ToDo: abstract multiple providers message body object
            Contact sender = new Contact() { Email = ((MailboxAddress)mMessage.From[0]).Address, Name = mMessage.From[0].Name };
            Contact[] to = new Contact[] { new Contact() { Email = ((MailboxAddress)mMessage.To[0]).Address, Name = mMessage.To[0].Name } };
            Message message = new Message()
            {
                Sender = sender,
                To = to,
                Subject = mMessage.Subject,
                HtmlContent = mMessage.HtmlBody
            };

            var json = JsonSerializer.Serialize(message);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("smtp/email", stringContent);
        }

        internal class Message
        {
            [JsonPropertyName("sender")]
            public Contact Sender { get; set; }

            [JsonPropertyName("to")]
            public Contact[] To { get; set; }

            [JsonPropertyName("subject")]
            public string Subject { get; set; }

            [JsonPropertyName("htmlContent")]
            public string HtmlContent { get; set; }
        }

        internal class Contact
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }
        }
    }
}
