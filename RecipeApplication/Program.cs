using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApplication
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
           .WriteTo.Console()
           .WriteTo.File("logs/RecipeApp.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Соединение неожиданно завершается(Host terminated unexpectedly)");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()//добавляем Serilog /*ConfigureLogging(builder=>builder.AddConsole())//- использ. стандартного поставщика журналирования*/
                .ConfigureLogging(builder => builder.AddSeq())
                .ConfigureWebHost/*Defaults*/(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseIIS();
                    webBuilder.UseIISIntegration();
                });
    }
}
