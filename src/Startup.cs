using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace smart_local
{
    /// <summary>
    /// Startup class for the ASP.NET Core application
    /// </summary>
    public class Startup
    {
        private static IServerAddressesFeature? _addresses;

        /// <summary>
        /// List of the webserver addresses in use.
        /// </summary>
        public static IServerAddressesFeature? Addresses => _addresses;

        // This method is called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// <summary>
        /// ConfigureServices method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method is called after the ConfigureServices method to configure the HTTP request pipeline.
        /// <summary>
        /// Configure method to set up the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var addresses = app.ServerFeatures.Get<IServerAddressesFeature>();

            if (addresses == null)
            {
                throw new Exception("No server addresses feature found.");
            }
            else
            {
                _addresses = addresses;
            }


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}