﻿using System;
using System.IO;
using FhirStarter.R4.Detonator.Core.Formatters;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Instigator.Core.Configuration;
using FhirStarter.R4.Instigator.Core.Extension;
using FhirStarter.R4.Instigator.Core.Helper;
using FhirStarter.R4.Instigator.Core.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            var appSettings =
                StartupConfigHelper.BuildConfigurationFromJson(Directory.GetCurrentDirectory(), "appSettings.json");

            var fhirStarterSettings =
                StartupConfigHelper.BuildConfigurationFromJson(AppContext.BaseDirectory, "FhirStarterSettings.json");
            FhirStarterConfig.SetupFhir(services, fhirStarterSettings, CompatibilityVersion.Version_2_2);

            services.Configure<FhirStarterSettings>(fhirStarterSettings.GetSection(nameof(FhirStarterSettings)));
            services.AddMvc(options =>
                {
                    options.OutputFormatters.Clear();
                    options.RespectBrowserAcceptHeader = true;
                    options.OutputFormatters.Add(new XmlFhirSerializerOutputFormatter());
                    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    options.OutputFormatters.Add(new JsonFhirFormatter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<IFhirService> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<HeaderValidation>();
            // To get OperationOutcome add this feature



            app.ConfigureExceptionHandler(logger);
            app.UseMvc();
        }
    }
}
