# Secrets - the quick tutorial

[Link](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.0&tabs=windows)

1. dotnet user-secrets init --project <Name>
2. Add keys
..* 
21. dotnet user-secrets set "Movies:ServiceApiKey" "12345"
22. Right click solution, select manage user secrets, edit the json that pops up
23. %APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json 
3. Startup.cs quick method
..* 
31. private (static) string _moviesApiKey = null;
32. _moviesApiKey = Configuration["Movies:ServiceApiKey"];
4. POCO method - create a class MovieSettings mapped to the JSON object
..* 
41. MovieSettings moviesConfig = Configuration.GetSection("Movies").Get<MovieSettings>();
42. _moviesApiKey2 = moviesConfig.ServiceApiKey;
5. Access your secret key e.g. Privacy PageModel
..* 
51. public string secretKey;
52. public void OnGet() => secretKey = Startup._moviesApiKey2;
53. <h2>@Model.secretKey</h2> <-- Don't do this 