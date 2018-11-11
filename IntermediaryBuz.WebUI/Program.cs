using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace IntermediaryBuz.WebUI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// refer below get start tutorial for using NLog in asp.net core:
			// https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2
			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

			try
			{
				CreateWebHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Application stopped because of exception.");
			}
			finally
			{
				NLog.LogManager.Shutdown();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.CaptureStartupErrors(true)
			.UseContentRoot(Environment.CurrentDirectory)
			.UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
			.UseStartup<Startup>()
			.UseIISIntegration()
			.UseKestrel()
			.ConfigureLogging(logging => {
				logging.ClearProviders();
				logging.SetMinimumLevel(LogLevel.Trace);
			})
			.UseNLog();
	}
}
