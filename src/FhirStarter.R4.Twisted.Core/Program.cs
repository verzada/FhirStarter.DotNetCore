﻿using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace FhirStarter.R4.Twisted.Core
{
    public class Program
    {
        public static void Main()
        {
            var hostname = Dns.GetHostName();
            var webHost = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging((hostingContext, logging) => { logging.AddLog4Net("log4net.config"); })
                // the .useUrls only applies when using dotnet run
                //.UseUrls($"http://{hostname}:5052")
                .UseStartup<Startup>()
                .Build();
            webHost.Run();
        }
    }
}
