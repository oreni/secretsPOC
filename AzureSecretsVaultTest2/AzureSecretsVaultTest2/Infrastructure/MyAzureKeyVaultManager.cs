using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureSecretsVaultTest2.InfraStructure
{
    public class MyAzureKeyVaultManager : IKeyVaultSecretManager
    {
        private readonly string _sharedPrefix = "SharedSecrets-";
        private readonly string _prefix;

        public MyAzureKeyVaultManager(string prefix)
        {
            _prefix = prefix;
        }

        public string GetKey(SecretBundle secret)
        {
            string res;
            string secertName = secret.SecretIdentifier.Name;
            if (secertName.StartsWith(_prefix))
            {
                res = secret.SecretIdentifier.Name.Substring(_prefix.Length);
            }
            else
            {
                res = secret.SecretIdentifier.Name.Substring(_sharedPrefix.Length);
            }
            return res.Replace("--", ConfigurationPath.KeyDelimiter);
        }

        public bool Load(SecretItem secret)
        {
            return secret.Identifier.Name.StartsWith(_prefix) || secret.Identifier.Name.StartsWith(_sharedPrefix);
        }
    }
}
