using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SecretsTest {
    public class Startup {
        // Declare your secrets here
        private string _moviesApiKey = null;
        public static string _moviesApiKey2 = null;
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            // Initialise your secrets here
            _moviesApiKey = Configuration["Movies:ServiceApiKey"];

            // Another method which is mapped to an object like how a JSON can be deserialised into an object:
            MovieSettings moviesConfig = Configuration.GetSection("Movies")
                                .Get<MovieSettings>();
            _moviesApiKey2 = moviesConfig.ServiceApiKey;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
            });

            string result = string.IsNullOrEmpty(_moviesApiKey) ? "Null" : "Not Null";
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Secret is {result}");
            });
        }
    }

    public class MovieSettings {
        public string ConnectionString { get; set; }
        public string ServiceApiKey { get; set; }
    }
}
