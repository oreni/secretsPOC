using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureSecretsVaultTest2.Models
{
    public interface IOktaSingletonClient
    {
        public OktaSettings GetOktaSettings();

    }
    public class OktaSingletonClient : IOktaSingletonClient, IDisposable
    {
        private OktaSettings _settings { get; set; }

        public OktaSingletonClient(OktaSettings settings)
        {
            _settings = settings;
        }

        public OktaSettings GetOktaSettings()
        {
            return _settings;
        }

        public void Dispose()
        {
            // Handle dispose to prepare to be a new object
        }
    }
}
