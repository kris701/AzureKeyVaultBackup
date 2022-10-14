using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyVaultBackup
{
    /// <summary>
    /// Requires the following things to be set in user secrets to work:
    ///     AZURE-KEY-VAULT-URI: URL to the key vault
    ///     AZURE-CLIENT-ID: Client ID of the app in azure
    ///     AZURE-CLIENT-SECRET: Secret to the client ID
    /// </summary>
    public class Program
    {
        async static Task Main(string[] args)
        {
            Console.WriteLine("Connecting to key vault...");
            KeyVaultBackupService<Program> service = new KeyVaultBackupService<Program>();
            Console.WriteLine("Fetching Keys...");
            var items = await service.GetKeys();
            Console.WriteLine("Saving to document");
            service.Save(items, $"{DateTime.Now.ToString("yyyy-MM-dd")}-azure-key-vault-backup.json");
            Console.WriteLine("Done!");
        }
    }
}
