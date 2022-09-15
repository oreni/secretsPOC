using AzureSecretsVaultTest2.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureSecretsVaultTest2.Models
{
    public class SalesForceSettings : Validatable
    {
        public string Version { get; set; }
        public string URI { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Version))
            {
                throw new Exception("SalesForceSettings.Version must not be null or empty");
            }

            // throws a UriFormatException if not a valid URL
            var uri = new Uri(URI);
        }
    }
}
