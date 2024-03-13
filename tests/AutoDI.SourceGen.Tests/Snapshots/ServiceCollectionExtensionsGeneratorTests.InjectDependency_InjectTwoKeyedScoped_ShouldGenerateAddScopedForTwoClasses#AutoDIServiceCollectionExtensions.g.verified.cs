//HintName: AutoDIServiceCollectionExtensions.g.cs
using Microsoft.Extensions.DependencyInjection;
using Tests.Services;

namespace Tests
{
	public static class AutoDI_ServiceCollectionExtensions
	{
		public static void AddAutoDI(this IServiceCollection services)
		{
			services.AddKeyedScoped<IService, KeyedService1>("1");
			services.AddKeyedScoped<IService, KeyedService2>("2");
		}
	}
}
