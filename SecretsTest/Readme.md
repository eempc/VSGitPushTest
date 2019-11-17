# Secrets - the quick tutorial

Secrets are only for development, not production

[Link](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.0&tabs=windows)

1. dotnet user-secrets init --project Name
2. Add keys 
	* dotnet user-secrets set "Movies:ServiceApiKey" "12345"
	* Right click solution, select manage user secrets, edit the json that pops up
	* %APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json

3. Startup.cs quick method
	* private (static) string _moviesApiKey = null;
	* _moviesApiKey = Configuration["Movies:ServiceApiKey"];

4. POCO method - create a class MovieSettings mapped to the JSON object 
	* MovieSettings moviesConfig = Configuration.GetSection("Movies").Get<MovieSettings>();
	* _moviesApiKey2 = moviesConfig.ServiceApiKey;

5. Access your secret key e.g. Privacy PageModel
	* public string secretKey;
	* public void OnGet() => secretKey = Startup._moviesApiKey2;
	* ```<h2>@Model.secretKey</h2>``` <-- Don't do this 

# For production, you will need Azure Vault to hold passwords securely

[Link1](https://stackoverflow.com/questions/40131672/storing-production-secrets-in-asp-net-core)

-------------------

# Vault Key quick tutorial

##Key Vault (CLI mostly)

1. https://shell.azure.com - You will be prompted to create a Storage if none exists
2. Create resource group if empty, e.g. az group create --name "MyResourceGroup" --location northeurope - already have one
3. Create key vault, e.g. az keyvault create --name "My-Uniquely-Named-Key-Vault" --resource-group "MyResourceGroup" --location northeurope
4. Add an example secret password, e.g. az keyvault secret set --vault-name "My-Uniquely-Named-Key-Vault" --name "ExamplePassword" --value "hVFkk965BuUv"
5. (Optional) show your secret: az keyvault secret show --name "ExamplePassword" --vault-name "<YourKeyVaultName>"
6. Access the password: https://<vault-name>.vault.azure.net/secrets/ExamplePassword

## In VS2019 app development

1. Install nuget packages:
*AppAuthentication
*Microsoft.Azure.KeyVault

2. In the CS file of a page, e.g. Privacy.cshtml.cs you will need this at the minimum:

```
public async Task OnGetAsync() {            
    VaultKeyExample = "Your app description page";
    //int retries = 0;
    //bool retry = false;
    try {
        AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
        KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        SecretBundle secret = await keyVaultClient.GetSecretAsync("https://Key-Vault-11223344.vault.azure.net/secrets/ExamplePassword").ConfigureAwait(false);
        VaultKeyExample = secret.Value;
    } catch (KeyVaultErrorException e) {
        VaultKeyExample = e.Message;
    }
}
```

* Also ensure you are on the right account, my ac.uk account by going to Tools > Options > Azure Service Authentication
* The local debug version will work fine now.

------------

3. Next is to authenticate an app (one that has been previously uploaded is probably best because it will have been named automatically)
4. In Azure console: az webapp identity assign --name "<YourAppName>" --resource-group "<YourResourceGroupName>"
5. Note the JSON principalId
6. az keyvault set-policy --name '<YourKeyVaultName>' --object-id <PrincipalId> --secret-permissions get list
7. Publish!

