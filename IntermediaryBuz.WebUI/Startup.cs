using AspNetCoreRateLimit;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace IntermediaryBuz.WebUI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// Which means this is the place help us to configure components of our application.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			// configuration for RateLimit begin
			// refer: https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup
			services.AddOptions();
			services.AddMemoryCache();
			services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
			services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
			services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
			services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
			// configuration for RateLimit ends.

			services.AddAntiforgery();
			ConfigureIoc(services);
			services.AddAutoMapper();
			// configuration for using Session starts
			services.AddDistributedMemoryCache();
			services.AddSession(options => {
				options.IdleTimeout = TimeSpan.FromMinutes(10);
				options.Cookie.HttpOnly = true;
			});
			// configuration for using Session ends.

			services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_0);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request procesing pipeline.
		// This is the DI entry to inject:
		// 1. IHostEnvironment to configure services by environment
		// 2. IConfiguration to read configuration
		// 3. ILoggerFactory to create a logger in Startup.ConfigureServices
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			// should register the limiting middleware before any other components except loggerFactory
			//app.UseIpRateLimiting();

			env.EnvironmentName = EnvironmentName.Production;

			if (env.IsDevelopment())
			{
				app.UseStatusCodePages();
				app.UseDeveloperExceptionPage();
				//app.UseExceptionHandler("~/Error/Exception/");
			}
			else
			{
				//app.UseStatusCodePagesWithRedirects("~/Error/Status/");
				// Global exception handling and logging
				app.UseExceptionHandler(errApp => {
					errApp.Run(async context => {
						var errFeature = context.Features.Get<IExceptionHandlerFeature>();
						var exception = errFeature.Error;
						var logger = loggerFactory.CreateLogger("AdaterCoreWeb");
						logger.LogError(exception, "Customized Exception");
						//context.Response.Redirect("~/Error/Exception/");
					});
				});
			}

			app.UseFileServer();
			app.UseSession();
			app.UseMvc(ConfigureRoute);
		}

		private void ConfigureRoute(IRouteBuilder routeBuilder)
		{
			routeBuilder.MapRoute("Home", "{controller=Home}/{action=Index}/{id?}");
		}

		private void ConfigureIoc(IServiceCollection services)
		{
			//services.AddScoped<COHS.AppServices.Interfaces.IDbService, SqlServerDbService>();
			//services.AddScoped<IAppUserService, AppUserService>();
			//services.AddScoped<ICommonService, CommonService>();
			//services.AddScoped<IDiagnosticService, DiagnosticsService>();
			//services.AddScoped<IMenuService, MenuService>();
			//services.AddScoped<IPatientService, PatientService>();
		}
	}
}
