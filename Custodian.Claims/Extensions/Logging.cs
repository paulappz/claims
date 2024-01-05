using Custodian.Claims.Extensions;
using Serilog;

namespace Custodian.Claims.Extensions
{
	public static class Logging
	{
		public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
		{
			var logger = new LoggerConfiguration()
		   .ReadFrom.Configuration(builder.Configuration)
		   .Enrich.FromLogContext()
		   .CreateLogger();
			builder.Logging.ClearProviders();
			builder.Logging.AddSerilog(logger);
			return builder;
		}
	}
}
