# Setting up Startup.cs

For the FhirStarter to work with all of the necessary dependencies, Startup.cs has to be configured.

For the development version, look at [Startup.cs](https://github.com/verzada/FhirStarter.DotNetCore/blob/master/src/FhirStarter.R4.Twisted.Core/Startup.cs). <br />
For the nuget setup version, look at [Startup.cs](https://github.com/verzada/FhirStarter.DotNetCore/blob/master/src/FhirStarter.R4.Twisted.Core.Nuget/Startup.cs).

## What needs to be added in the IServiceCollection

### FhirSetup

ConfigureServices runs a customized method to which adds config files, necessary dlls and ouputs for the web project.
The dlls, detonator and instigator needs to be accessible in memory, .Net Core handles from our experience, dlls differently from .Net Framework, so the dll's are are added at startup.

#### FhirStarterSettings
The FhirStarterSettings needs to be accessible for the FhirController, so the dll(s) containing the IFhirService(s) can be known to the service and added to memory.

#### Formats
By default we recommend removing the standard output formats and add the necessary formats afterwards.

#### The Fhir Controller

The Fhir Controller is in the Instigator library and must be added as an AddApplicationPart as well as AddControllerAsServices() in order for the FhirController to work. If you want to ovveride the FhirStarter controller with a your custom controller, it is possible to do it here, by f.ex. adding the controller directory into the web project and ignoring the AddControllerAsServices call.

### Code example for FhirSetup used by 

```c#
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
```

## What needs to be added in the Configure Section

### ExceptionHandling

In .Net Core, the best way of customizing the exception handling, seems to be in the Configure method.
The solution we've found so far, which gives the developer a chance of changing the ouput from the exception into an OperationOutcome, is to configure it in the Startup.cs file.

We've chosen to serialize the response to json by default, but it is possible to change it to what is necessary for any given scenario.

### Logging

We've chosen to use log4net since it's easier and quicker to configure.
In order to configure log4net sucessfully, make sure the log4net.config file is accessible from the root folder of the web project and that the Main method in the Program.cs file has the line 

```c#
  .ConfigureLogging((hostingContext, logging) => { logging.AddLog4Net("log4net.config"); })
```

The log4net.config file is customizable to any extent. In our version of config file, info and warn/errors are logged to two different files. It is possible to just have one file for the same purpose. For more information about log4net look at the [guide from apache](https://logging.apache.org/log4net/release/manual/configuration.html)

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <appender name="LogFileAppenderInfo" type="log4net.Appender.FileAppender">
    <param name="File" value="c:\logs\FhirStarter.STU3.Twisted.Core_info.log" />
    <param name="AppendToFile" value="true" />
    <layout type="log4net.Layout.PatternLayout">
      <param name="Header" value="" />
      <param name="Footer" value="" />
      <param name="ConversionPattern" value="%d [%t] %-5p %m%n" />
    </layout>
    <filter type="log4net.Filter.LevelRangeFilter">
        <levelMin value="INFO"/>
        <levelMax value="INFO" />
      </filter>
  </appender>
  <appender name="LogFileAppenderWarn" type="log4net.Appender.FileAppender">
    <param name="File" value="c:\logs\FhirStarter.STU3.Twisted.Core_warn.log" />
    <param name="AppendToFile" value="true" />
    <layout type="log4net.Layout.PatternLayout">
      <param name="Header" value="" />
      <param name="Footer" value="" />
      <param name="ConversionPattern" value="%d [%t] %-5p %m%n" />
    </layout>
    <filter type="log4net.Filter.LevelRangeFilter">
      <levelMin value="WARN"/>
    </filter>
  </appender>
  <root>
    <appender-ref ref="LogFileAppenderInfo" />
    <appender-ref ref="LogFileAppenderWarn" />
  </root>
</log4net>
```

### Code Example for Configure

```c#
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
```