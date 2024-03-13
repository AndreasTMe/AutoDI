//HintName: AutoDIServiceCollectionExtensions.g.cs
using Microsoft.Extensions.DependencyInjection;
using Tests.Services;

namespace Tests
{
	public static class AutoDI_ServiceCollectionExtensions
	{
		public static void AddAutoDI(this IServiceCollection services)
		{
			services.AddKeyedSingleton<IService, KeyedService1>("1");
			services.AddKeyedSingleton<IService, KeyedService2>("2");
		}
	}
}
