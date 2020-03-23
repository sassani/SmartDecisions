using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntraServicesApi
{
    public class MailServiceApi
    {
        private readonly HttpClient client = new HttpClient();
        public MailServiceApi(string apikey)
        {
            client.DefaultRequestHeaders.Clear();
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
                    email,
                    title = "Email Verification Link",
                    description = $"Please verify your email by click the below button({email})",
                    url = token,
                    label = "Verify"
                };
                var json = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("actionlink", stringContent);
                if (!response.IsSuccessStatusCode) throw new Exception("Mail Service Failed");
            }
            catch (HttpRequestException e)
            {

                throw;
            }
        }

        public async Task SendForgotPasswordLink(string email, string token)
        {
            try
            {
                Object data = new
                {
                    email,
                    title = "Forgot Password request",
                    description = "Click below button to change your password",
                    url = token,
                    label = "Change My Password"
                };
                var json = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("actionlink", stringContent);
                if (!response.IsSuccessStatusCode) throw new Exception("Mail Service Failed");
            }
            catch (HttpRequestException e)
            {

                throw;
            }
        }
    }
}
