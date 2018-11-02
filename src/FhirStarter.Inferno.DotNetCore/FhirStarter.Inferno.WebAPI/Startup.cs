using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FhirStarter.Inferno.WebAPI.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

            private void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
                IHttpContextAccessor contextAccessor)
            {
                var appSettings =
                    StartupConfigHelper.BuildConfiguration(Directory.GetCurrentDirectory(), "appSettings.json");
                var fhirStarterSettings =
                    StartupConfigHelper.BuildConfiguration(Directory.GetCurrentDirectory(), "FhirStarterSettings.json");

                //config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
                //config.Formatters.JsonFormatter.SupportedMediaTypes.Clear();
                //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xacml+json"));
                //config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                FhirStarterConfig.SetupFhir(app, env,loggerFactory, contextAccessor, appSettings, fhirStarterSettings);
                
            }
    }
}
