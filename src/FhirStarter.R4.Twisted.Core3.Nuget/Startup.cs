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
using Microsoft.Extensions.Hosting;
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
            FhirDotnet3Setup(services);
        }

        // Copy this method
        public void FhirDotnet3Setup(IServiceCollection services)
        {
            var appSettings =
                StartupConfigHelper.BuildConfigurationFromJson(AppContext.BaseDirectory, "appsettings.json");
            FhirStarterConfig.SetupFhir(services, appSettings, CompatibilityVersion.Version_3_0);

            var detonator = FhirStarterConfig.GetDetonatorAssembly();
            var instigator = FhirStarterConfig.GetInstigatorAssembly();

            services.Configure<FhirStarterSettings>(appSettings.GetSection(nameof(FhirStarterSettings)));
            services.AddRouting();
            services.AddControllers(controller =>
                {
                    controller.OutputFormatters.Clear();
                    controller.RespectBrowserAcceptHeader = true;
                    //controller.OutputFormatters.Add(new JsonFhirFormatterDotNetCore3());
                    //controller.OutputFormatters.Add(new XmlFhirSerializerOutputFormatterDotNetCore3());
                    controller.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                })
                .AddApplicationPart(instigator).AddApplicationPart(detonator).AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddHttpContextAccessor();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<IFhirService> logger)
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
                    context.Response.ContentType = "application/json+fhir";
                    await context.Response.WriteAsync(result);
                }
            }));

            app.ConfigureExceptionHandler(logger);
            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
