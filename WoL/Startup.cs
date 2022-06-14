using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "We're fine with english texts regarding configuration and startup.")]
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
                services.AddDbContextFactory<SqlServerDbContext>(options =>
                    options.UseSqlServer(tsql));
                services.AddSingleton<CovariantDbContextFactory<ApplicationDbContext, SqlServerDbContext>>();
                services.AddSingleton<IDbContextFactory<ApplicationDbContext>>(
                    sp => sp.GetRequiredService<CovariantDbContextFactory<ApplicationDbContext, SqlServerDbContext>>());
            }
            else
            {
                services.AddDbContextFactory<SqliteDbContext>(options =>
                    options.UseSqlite(sqlite));
                services.AddSingleton<CovariantDbContextFactory<ApplicationDbContext, SqliteDbContext>>();
                services.AddSingleton<IDbContextFactory<ApplicationDbContext>>(
                    sp => sp.GetRequiredService<CovariantDbContextFactory<ApplicationDbContext, SqliteDbContext>>());
            }
        }

        private void ConfigureAppInsights(IServiceCollection services)
        {
            var aiStorageFolder = Configuration.GetValue<string>("ApplicationInsightsStorageFolder", null);
            if (aiStorageFolder != null)
            {
                // For Linux OS
                services.AddSingleton<ITelemetryChannel>(new ServerTelemetryChannel { StorageFolder = aiStorageFolder });
            }
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAppInsights(services);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Argumente von öffentlichen Methoden validieren", Justification = "Called by the runtime, app will not be null")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            if (Configuration.GetValue<bool>("RequireHttps", true))
                app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            if (Configuration.GetValue<bool>("AutoUpdateDatabase", false))
                UpdateDatabase(app);
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var fac = serviceScope.ServiceProvider.GetService<IDbContextFactory<ApplicationDbContext>>();
            using var context = fac.CreateDbContext();
            // this could be async but configuration of the dependencies isn't
            // https://stackoverflow.com/a/37573402/1200847
            context.Database.Migrate();
        }
    }
}
