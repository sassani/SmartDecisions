using Newtonsoft.Json;
using Shared;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntraServicesApi
{
    public class MailServiceApi
    {
        private readonly HttpClient client = new HttpClient();
        public MailServiceApi(string apikey, string mailServerUrl)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(GLOBAL_CONSTANTS.SHARED_API_KEY_HEADER_NAME, apikey);
            client.BaseAddress = new Uri(mailServerUrl);        }

        public async Task SendVerificationEmail(string email, string token)
        {
            await SendActionLink(new
            {
                email,
                title = "Email Verification Link",
                description = $"Please verify your email by click the below button({email})",
                url = token,
                label = "Verify"
            });
        }

        public async Task SendForgotPasswordLink(string email, string token)
        {
            await SendActionLink(new
            {
                email,
                title = "Forgot Password request",
                description = "Please click below button to change your password",
                url = token,
                label = "Change My Password"
            });
        }

        private async Task SendActionLink(object data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("actionlink", stringContent);
                if (!response.IsSuccessStatusCode) throw new IntraServiceException(HttpStatusCode.InternalServerError, "Mail Server Error", $"The response status: ({response.StatusCode})");
            }
            catch (IntraServiceException err)
            {
                throw err;
            }
            catch (Exception)
            {
                throw new IntraServiceException(HttpStatusCode.InternalServerError, "Mail Server Error", "Mail server does not respond");
            }

        }
    }
}
