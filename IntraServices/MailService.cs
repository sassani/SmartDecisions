using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntraServices
{
    public class MailService
    {
        private static readonly HttpClient client = new HttpClient();
        public MailService(string apikey)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("apiKey", apikey);
            client.BaseAddress = new Uri("https://mailservice.ardavansassani.com");
        }
        public async Task SendVerificationEmail(string email, string token)
        {
            try
            {
                Object data = new
                {
                    email = email,
                    title = "Email Verification Link",
                    description = $"Please verify your email by click the below button({email})",
                    url = token,
                    label = "Verify"
                };
                var json = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("actionlink", stringContent);
            }
            catch (HttpRequestException e)
            {

                throw;
            }
        }
    }
}
