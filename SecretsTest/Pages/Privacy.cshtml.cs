using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;

namespace SecretsTest.Pages {
    public class PrivacyModel : PageModel {
        private readonly ILogger<PrivacyModel> _logger;

        // Secret key is here
        //public string secretKey; // This is a development local key, not the vault key
        public string VaultKeyExample { get; set; }

        public PrivacyModel(ILogger<PrivacyModel> logger) {
            _logger = logger;
        }

        //public void OnGet() {
        //    secretKey = Startup._moviesApiKey2;
        //}

        public async Task OnGetAsync() {
            //secretKey = Startup._moviesApiKey2;
            VaultKeyExample = "Your app description page";
            int retries = 0;
            bool retry = false;
            try {
                AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
                KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                SecretBundle secret = await keyVaultClient.GetSecretAsync("https://Key-Vault-11223344.vault.azure.net/secrets/ExamplePassword").ConfigureAwait(false);
                VaultKeyExample = secret.Value;
            } catch (KeyVaultErrorException e) {
                VaultKeyExample = e.Message;
            }
        }

        // This method implements exponential backoff if there are 429 errors from Azure Key Vault
        private static long getWaitTime(int retryCount) {
            long waitTime = ((long)Math.Pow(2, retryCount) * 100L);
            return waitTime;
        }

        // This method fetches a token from Azure Active Directory, which can then be provided to Azure Key Vault to authenticate
        public async Task<string> GetAccessTokenAsync() {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://vault.azure.net");
            return accessToken;
        }
    }
}
