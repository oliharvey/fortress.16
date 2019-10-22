using System;
using System.Web.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
namespace KaiOs_Registration.Helpers
{
    public static class ApiHelper
    {
        public static async Task<HttpResponseMessage> CallApi(string method, object request)
        {
            try
            {
                String result = String.Empty;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["FortressApiUrl"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", Encryption.GetTodaysEncryptedToken());

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(request));
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    
                    HttpResponseMessage response = await client.PostAsync(String.Format(method), stringContent);
                   await response.Content.ReadAsStringAsync();

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}