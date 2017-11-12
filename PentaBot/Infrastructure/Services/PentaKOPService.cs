using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PentaBot.Infrastructure.Services
{
    public class PentaKOPService : IPentaKOPService
    {
        private static readonly HttpClient client = new HttpClient();
        public async Task AddActivity(string comment)
        {
            var loginValues = new Dictionary<string, string>
                {
                   { "NazwaUzytkownika", ConfigurationManager.AppSettings["NazwaUzytkownika"].ToString() },
                   { "Haslo", ConfigurationManager.AppSettings["Haslo"].ToString() },
                   { "s1", ConfigurationManager.AppSettings["s1"].ToString() }
                };
            var newEntryValues = new Dictionary<string, string>
                {
                   { "ed2", ConfigurationManager.AppSettings["ed2"].ToString() },
                   { "k1e2", ConfigurationManager.AppSettings["k1e2"].ToString() },
                   { "k2e2", ConfigurationManager.AppSettings["k2e2"].ToString() },
                   { "k3e2", ConfigurationManager.AppSettings["k3e2"].ToString() },
                   { "k4e2", ConfigurationManager.AppSettings["k4e2"].ToString() },
                   //{ "k5e2", ConfigurationManager.AppSettings["k5e2"].ToString() },
                   { "k5e2", comment },
                   { "k7e2", ConfigurationManager.AppSettings["k7e2"].ToString() },
                   { "lastId", ConfigurationManager.AppSettings["lastId"].ToString() }
                };
            var loginContent = new FormUrlEncodedContent(loginValues);
            var newEntryContent = new FormUrlEncodedContent(newEntryValues);

            var responseFromLoginRequest = await client.PostAsync(ConfigurationManager.AppSettings["loginUrl"].ToString(), loginContent);
            var responseFromNewEntryRequest = await client.PostAsync(ConfigurationManager.AppSettings["newActivityUrl"].ToString(), newEntryContent);

            var loginResponseString = await responseFromLoginRequest.Content.ReadAsStringAsync();
            var newEntryResponseString = await responseFromNewEntryRequest.Content.ReadAsStringAsync();
        }
    }
}