# Azure Key Vault Backup
Simple program to save all the keys from a Azure Key Vault into a json document.
Just remember to add these keys to the usersecrets `secrets.json` file:
```json
{
  "AZURE-KEY-VAULT-URI": "URL to the key vault",
  "AZURE-CLIENT-ID": "Client ID of the app in azure",
  "AZURE-CLIENT-SECRET": "Secret to the client ID"
}
```
