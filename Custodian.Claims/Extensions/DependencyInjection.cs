using Custodian.Claims.ClaimsProcessing;

namespace Custodian.Claims.Extensions
{
	public static class DependencyInjection
	{
		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddControllersWithViews();
			builder.Services.AddScoped<IClaimsRequestManager, ClaimsRequestManager>();
		}
	}
}
