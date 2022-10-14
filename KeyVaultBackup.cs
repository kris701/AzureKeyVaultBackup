using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyVaultBackup
{
    public class KeyVaultBackup
    {
        public List<KeyVaultItem> Items { get; set; }

        public KeyVaultBackup()
        {
            Items = new List<KeyVaultItem>();
        }
    }
}
