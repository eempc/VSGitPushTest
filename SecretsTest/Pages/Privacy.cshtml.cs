using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SecretsTest.Pages {
    public class PrivacyModel : PageModel {
        private readonly ILogger<PrivacyModel> _logger;

        // Secret key is here
        public string secretKey;

        public PrivacyModel(ILogger<PrivacyModel> logger) {
            _logger = logger;
        }

        public void OnGet() {
            secretKey = Startup._moviesApiKey2;
        }
    }
}
