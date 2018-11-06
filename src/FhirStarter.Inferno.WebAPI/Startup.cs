using System.IO;
using FhirStarter.Bonfire.DotNetCore.Interface;
using FhirStarter.Inferno.WebAPI.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FhirStarter.Inferno.WebAPI
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
                StartupConfigHelper.BuildConfiguration(Directory.GetCurrentDirectory(), "appSettings.json");
            var fhirStarterSettings =
                StartupConfigHelper.BuildConfiguration(Directory.GetCurrentDirectory(), "FhirStarterSettings.json");

            services.Configure<FhirStarterSettings>(fhirStarterSettings.GetSection(nameof(FhirStarterSettings)));
            services.AddMvc(config =>
                {
                    // Add XML Content Negotiation
                    config.RespectBrowserAcceptHeader = true;
                    config.InputFormatters.Add(new XmlSerializerInputFormatter());
                    config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                })
                //.AddXmlSerializerFormatters()
                //.AddXmlDataContractSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            FhirStarterConfig.SetupFhirServices(services, appSettings, fhirStarterSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<IFhirService> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler(logger);
            app.UseMvc();
        }
    }
}
