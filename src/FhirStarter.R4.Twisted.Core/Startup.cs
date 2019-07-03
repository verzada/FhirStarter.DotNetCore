using System;
using FhirStarter.R4.Detonator.Core.Filter;
using FhirStarter.R4.Detonator.Core.Formatters;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Instigator.Core.Configuration;
using FhirStarter.R4.Instigator.Core.Extension;
using FhirStarter.R4.Instigator.Core.Helper;
using FhirStarter.R4.Instigator.Core.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FhirStarter.R4.Twisted.Core
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
            FhirSetup(services);
        }

        // Copy this method
        private void FhirSetup(IServiceCollection services)
        {
            var appSettings =
                StartupConfigHelper.BuildConfigurationFromJson(AppContext.BaseDirectory, "appsettings.json");
            FhirStarterConfig.SetupFhir(services, appSettings, CompatibilityVersion.Version_2_2);

            var detonator = FhirStarterConfig.GetDetonatorAssembly();
            var instigator = FhirStarterConfig.GetInstigatorAssembly();

            services.Configure<FhirStarterSettings>(appSettings.GetSection(nameof(FhirStarterSettings)));
            services.AddMvc(options =>
                {
                    options.OutputFormatters.Clear();
                    options.RespectBrowserAcceptHeader = true;
                    options.OutputFormatters.Add(new XmlFhirSerializerOutputFormatter());
                    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    options.OutputFormatters.Add(new JsonFhirFormatter());
                       
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                })
                .AddApplicationPart(instigator).AddApplicationPart(detonator).AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<IFhirService> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                if (feature?.Error != null)
                {
                    var exception = feature.Error;

                    var operationOutcome = ErrorHandlingMiddleware.GetOperationOutCome(exception, true);
                    var fhirJsonConverter = new FhirJsonSerializer();
                    var result = fhirJsonConverter.SerializeToString(operationOutcome);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }
            }));

            app.ConfigureExceptionHandler(logger);
            app.UseMvc();
        }
    }
}
