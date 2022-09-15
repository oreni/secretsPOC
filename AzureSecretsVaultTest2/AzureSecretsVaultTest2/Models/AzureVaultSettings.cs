using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureSecretsVaultTest2.Models
{
    public class AzureVaultSettings
    {
        public bool UseVault { get; set; }
        public string VaultName { get; set; }
        public string ApplicationClientId { get; set; }
        public string SecretValue { get; set; }
        public int ReloadIntervalSeconds { get; set; }
        public string KeyNamePrefix { get; set; }
    }
}
