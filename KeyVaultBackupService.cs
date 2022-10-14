using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureKeyVaultBackup
{
    public class KeyVaultBackupService<T> where T : class
    {
        private static string AzureKeyVaultURI = "AZURE-KEY-VAULT-URI";
        private static string AzureClientID = "AZURE-CLIENT-ID";
        private static string AzureClientSecret = "AZURE-CLIENT-SECRET";

        private IConfiguration _configuration;
        private KeyVaultClient _client;

        public KeyVaultBackupService()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<T>();
            _configuration = builder.Build();
            builder.AddAzureKeyVault(_configuration[AzureKeyVaultURI], _configuration[AzureClientID], _configuration[AzureClientSecret]);
            _configuration = builder.Build();

            TokenCredential tokenCredential = new DefaultAzureCredential();
            _client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                    async (string auth, string res, string scope) => {
                        var authContext = new AuthenticationContext(auth);
                        var credential = new ClientCredential(_configuration[AzureClientID], _configuration[AzureClientSecret]);
                        AuthenticationResult result = await authContext.AcquireTokenAsync(res, credential);
                        if (result == null)
                            throw new InvalidOperationException("Failed to retrieve token");
                        return result.AccessToken;
                    }
                ));
        }

        public async Task<KeyVaultBackup> GetKeys()
        {
            KeyVaultBackup returnItem = new KeyVaultBackup();
            var list = await _client.GetSecretsAsync(_configuration[AzureKeyVaultURI]);
            foreach(var keyItem in list)
            {
                var secret = await _client.GetSecretAsync(_configuration[AzureKeyVaultURI], keyItem.Identifier.Name);
                returnItem.Items.Add(new KeyVaultItem()
                {
                    Key = keyItem.Identifier.Name,
                    Value = secret.Value
                });
            }
            return returnItem;
        }
        public void Save(KeyVaultBackup backup, string path)
        {
            string saveString = JsonSerializer.Serialize(backup, options: new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(path, saveString);
        }
    }
}
