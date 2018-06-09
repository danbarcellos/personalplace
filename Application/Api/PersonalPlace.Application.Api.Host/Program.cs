using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Pactor.Infra.Crosscutting.LogCore;

namespace PersonalPlace.Application.Api.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogBootstrap.Configure();
            var logger = LogBootstrap.GetLogger(typeof(Program));
            var machineName = Environment.MachineName;

            logger.Info(() => $"Starting Personal Place API host at {machineName}");

            var host = BuildWebHost(args);
            host.Run();

            logger.Info(() => $"Stopping Personal Place API host at {machineName}");
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .Build();
        }
    }
}
