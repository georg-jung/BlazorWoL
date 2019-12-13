using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WoL.Data;
using WoL.Services;

namespace WoL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureDbContext(IServiceCollection services)
        {
            var tsql = Configuration.GetConnectionString("TsqlConnection");
            var sqlite = Configuration.GetConnectionString("SqliteConnection");
            if (string.IsNullOrEmpty(tsql) && string.IsNullOrEmpty(sqlite))
                throw new InvalidOperationException("You need to configure either TsqlConnection or SqliteConnection.");
            if (!string.IsNullOrEmpty(tsql) && !string.IsNullOrEmpty(sqlite))
                throw new InvalidOperationException("You need to configure either TsqlConnection or SqliteConnection, but not both.");

            if (!string.IsNullOrEmpty(tsql))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(tsql));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(sqlite));
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDbContext(services);
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddTransient<IHostService, HostService>();
            services.AddTransient<IWakeService, WakeService>();
            services.AddTransient<IcmpPingService>();
            services.AddTransient<RdpPortPingService>();
            services.AddTransient<IPingService, CompositePingService>();
            services.AddTransient<IAddressLookupService, AddressLookupService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
