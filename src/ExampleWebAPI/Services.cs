using AutoDI.Attributes;

using ExampleWebAPI.InnerNamespace;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI
{
    [InjectService(typeof(ITestService), ServiceLifetime.Singleton, serviceNamespace: "ExampleWebAPI.InnerNamespace")]
    public sealed class TestService : ITestService
    {
        public Guid GetTestGuid { get; } = Guid.NewGuid();
    }
    
    /*********************/
    /* Singleton Service */
    /*********************/

    public interface ISingletonService
    {
        Guid GetSingletonGuid { get; }
    }

    [InjectService(typeof(ISingletonService), ServiceLifetime.Singleton)]
    public sealed class SingletonService : ISingletonService
    {
        private readonly ITestService _testService;

        public Guid GetSingletonGuid { get; } = Guid.NewGuid();

        public SingletonService(ITestService testService)
        {
            _testService = testService;

            Console.WriteLine($"TestService: {_testService.GetTestGuid}");
        }
    }

    /*********************/
    /* Scoped Service    */
    /*********************/

    public interface IScopedService
    {
        Guid GetScopedGuid { get; }
    }

    [InjectService(typeof(IScopedService), ServiceLifetime.Scoped)]
    public sealed class ScopedService : IScopedService
    {
        public Guid GetScopedGuid { get; } = Guid.NewGuid();
    }

    /*********************/
    /* Transient Service */
    /*********************/

    public interface ITransientService
    {
        Guid GetTransientGuid { get; }
    }

    [InjectService(typeof(ITransientService), ServiceLifetime.Transient)]
    public sealed class TransientService : ITransientService
    {
        public Guid GetTransientGuid { get; } = Guid.NewGuid();
    }

    /*********************/
    /* Abstract Service  */
    /*********************/

    public abstract class AbstractService
    {
        public abstract Guid GetAbstractGuid { get; }
    }

    [InjectService(typeof(AbstractService), ServiceLifetime.Singleton)]
    public sealed class ChildService : AbstractService
    {
        public override Guid GetAbstractGuid { get; } = Guid.NewGuid();
    }

    /*********************/
    /* Self Service      */
    /*********************/

    [InjectService(typeof(SelfService), ServiceLifetime.Singleton)]
    public sealed class SelfService
    {
        public Guid GetSelfGuid { get; } = Guid.NewGuid();
    }
}

namespace ExampleWebAPI.InnerNamespace
{
    public interface ITestService
    {
        Guid GetTestGuid { get; }
    }
}