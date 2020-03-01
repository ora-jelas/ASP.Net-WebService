/*
 * https://stackoverflow.com/questions/37346383/hosting-asp-net-core-as-windows-service
 * sc create SimpleRestApi binPath= "D:\C#\Projects\SimpleRestApi\SimpleRestApi\bin\Debug\netcoreapp3.1\SimpleRestApi.exe"
 */
/*
 * git push https://ora-jelas:...%21%21@github.com/ora-jelas/ASP.Net-WebService.git master
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//<<
using System.Diagnostics;
using Microsoft.AspNetCore;
using System.ServiceProcess;
//>>
using Microsoft.AspNetCore.Hosting;
//<<
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
//>>
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SimpleRestApi
{
    public class Program
    {
        const string CAppUrl = "https://localhost:5001";

        //allows running VS without elevation when debugging
#if DEBUG
        static readonly string baseAddress = "https://localhost:5001/";
#else
        static readonly string baseAddress = "https://*:5001/";
#endif

        public static void Main(string[] args)
        {
            const string CDebugArg = "--debug";

            bool isService = !Debugger.IsAttached && !args.Contains(CDebugArg);

            //if (isService)
            //{
            //    var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            //    var pathToContentRoot = System.IO.Path.GetDirectoryName(pathToExe);
            //    System.IO.Directory.SetCurrentDirectory(pathToContentRoot);
            //}

            //CreateHostBuilder(args).Build().Run();
            //IWebHost webHost = CreateWebHostBuilder(
            //    args.Where(x => x != CDebugArg).ToArray()
            //).Build();
            IWebHostBuilder webHostBuilder = CreateWebHostBuilder(
                args.Where(x => x != CDebugArg).ToArray()
            );//.UseUrls(CAppUrl);

            IWebHost webHost = webHostBuilder.Build();

            if (isService)
            {
                //webHost.RunAsCustomService();
                webHost.RunAsService();
            }
            else
            {
                webHost.Run();
            }
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Listen(System.Net.IPAddress.Loopback, 5001, listenOptions =>
                    {
                        /*
                        string exePath = System.IO.Path.GetDirectoryName(
                            System.Reflection.Assembly.GetExecutingAssembly().Location
                        );
                        listenOptions.UseHttps(
                            exePath + "\\" + "<any exported certificate file>.<p12/pfx>",
                            "Pass-123"
                        );
                        */
                        listenOptions.UseHttps("D:\\Downloads\\CertASP.pfx", "Pass-123");
                        //listenOptions.UseHttps("D:\\C#\\Projects\\localhostSvc.pfx", "Pass-123");
                    });
                })
                .UseStartup<Startup>();
        }
        /*
        // Below is possible alternative to keep using the default plain CreateHostBuilder(args).Build().Run() instead of 
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Loopback, 5000);
                        serverOptions.Listen(IPAddress.Loopback, 5001, listenOptions =>
                        {
                            listenOptions.UseHttps("testCert.pfx", "testPassword");
                        });
                    })
                    .UseStartup<Startup>();
                });
        */
    }

    /*
    public static class WebHostServiceExtensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            CustomWebHostService webHostService = new CustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }

    internal class CustomWebHostService : WebHostService
    {
        private readonly ILogger logger;

        public CustomWebHostService(IWebHost host) : base(host)
        {
            logger = host.Services
                .GetRequiredService<ILogger<CustomWebHostService>>();
        }

        protected override void OnStarting(string[] args)
        {
            logger.LogDebug("OnStarting method called.");
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            logger.LogDebug("OnStarted method called.");
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            logger.LogDebug("OnStopping method called.");
            base.OnStopping();
        }
    }
    */
}
