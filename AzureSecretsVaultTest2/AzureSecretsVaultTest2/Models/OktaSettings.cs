using AzureSecretsVaultTest2.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureSecretsVaultTest2.Models
{
    public class OktaSettings : Validatable
    {
        [Required]        
        public string ClientId { get; set; }
        [Required]
        public string Domain { get; set; }

        public Dictionary<string,string> Applications { get; set; }

    }
}
