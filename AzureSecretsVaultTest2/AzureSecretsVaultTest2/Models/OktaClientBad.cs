using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureSecretsVaultTest2.Models
{
    public interface IOktaClientBad
    {
        public OktaSettings GetOktaSettings();

    }
    public class OktaClientBad : IOktaClientBad
    {
        private OktaSettings _settings { get; set; }

        public OktaClientBad(OktaSettings settings)
        {
            _settings = settings;
        }

        public OktaSettings GetOktaSettings()
        {
            return _settings;
        }
        public void SetOktaSettings(OktaSettings settings)
        {
            _settings = settings;
        }

    }
}
