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