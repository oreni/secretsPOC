using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureSecretsVaultTest2.Models
{
    public interface IOktaClientWHttp
    {
        public HttpClient GetOktaSettings();

    }
    public class OktaClientWHttp : IOktaClientWHttp
    {
        private OktaSettings _settings { get; set; }
        public HttpClient _httpClient { get; set; }

        public OktaClientWHttp(HttpClient httpClient, OktaSettings settings)
        {
            _settings = settings;
            _httpClient = httpClient;
        }

        public HttpClient GetOktaSettings()
        {
            return _httpClient;
        }
        public void SetOktaSettings(OktaSettings settings)
        {
            _settings = settings;
        }

    }
}
