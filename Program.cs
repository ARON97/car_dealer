using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace cars
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            // var host = new WebHostBuilder()
            //     // Telling to use Kestral HTTP service
            //     .UseKestral()
            //     // Specifying the root of the application
            //     .UseContentRoot(Directory.GetCurrentDirection)
            //     // Enabling IIS Integration
            //     .UseIISIntegration()
            //     // Specifying the startup class
            //     .UseStartup<Startup>()
            //     .Build();
            // // Finally when we build the host we run it
            // host.Run() // at this point Kestral starts listening on localhpst:5000 
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
