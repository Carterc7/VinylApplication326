using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace VinylApplication326 // Change to your actual namespace
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add in-memory caching for session
            services.AddDistributedMemoryCache();

            // Configure session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set a timeout for the session
                options.Cookie.HttpOnly = true; // Make the cookie accessible only to the server
                options.Cookie.IsEssential = true; // Make the session cookie essential
            });

            // Add MVC support
            services.AddControllersWithViews(); // or services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // Enable session handling

            app.UseAuthorization(); // Use authorization middleware

            app.UseEndpoints(endpoints =>
            {
                // Configure default route
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

