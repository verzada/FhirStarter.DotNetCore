using System;
using System.IO;
using FhirStarter.STU3.Detonator.Core.Interface;
using FhirStarter.STU3.Instigator.Core.Configuration;
using FhirStarter.STU3.Instigator.Core.Extension;
using FhirStarter.STU3.Instigator.Core.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FhirStarter.STU3.Twisted.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings =
                StartupConfigHelper.BuildConfigurationFromJson(Directory.GetCurrentDirectory(), "appSettings.json");

            var fhirStarterSettings =
                StartupConfigHelper.BuildConfigurationFromJson(AppContext.BaseDirectory, "FhirStarterSettings.json");
            //FhirStarterConfig.SetupFhirServices(services, fhirStarterSettings);
            FhirStarterConfig.SetupFhir(services, fhirStarterSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<IFhirService> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<HeaderValidation>();
            // To get OperationOutcome add this feature
            app.ConfigureExceptionHandler(logger);

            app.UseMvc();
        }
    }
}
