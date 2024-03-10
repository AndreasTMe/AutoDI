using AutoDI.Attributes;

using ExampleWebAPI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoDI();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/singleton", ([FromServices] ISingletonService service) => service.GetSingletonGuid);
app.MapGet("/scoped", ([FromServices] IScopedService service) => service.GetScopedGuid);
app.MapGet("/transient", ([FromServices] ITransientService service) => service.GetTransientGuid);
app.MapGet("/abstract", ([FromServices] AbstractService service) => service.GetAbstractGuid);
app.MapGet("/self", ([FromServices] SelfService service) => service.GetSelfGuid);

app.Run();

namespace ExampleWebAPI
{
    /*********************/
    /* Singleton Service */
    /*********************/

    [IsDependency]
    public interface ISingletonService
    {
        Guid GetSingletonGuid { get; }
    }

    [InjectDependency(typeof(ISingletonService), ServiceLifetime.Singleton)]
    public sealed class SingletonService : ISingletonService
    {
        public Guid GetSingletonGuid { get; } = Guid.NewGuid();
    }

    /*********************/
    /* Scoped Service    */
    /*********************/

    [IsDependency]
    public interface IScopedService
    {
        Guid GetScopedGuid { get; }
    }

    [InjectDependency(typeof(IScopedService), ServiceLifetime.Scoped)]
    public sealed class ScopedService : IScopedService
    {
        public Guid GetScopedGuid { get; } = Guid.NewGuid();
    }

    /*********************/
    /* Transient Service */
    /*********************/

    [IsDependency]
    public interface ITransientService
    {
        Guid GetTransientGuid { get; }
    }

    [InjectDependency(typeof(ITransientService), ServiceLifetime.Transient)]
    public sealed class TransientService : ITransientService
    {
        public Guid GetTransientGuid { get; } = Guid.NewGuid();
    }

    /*********************/
    /* Abstract Service  */
    /*********************/

    [IsDependency]
    public abstract class AbstractService
    {
        public abstract Guid GetAbstractGuid { get; }
    }

    [InjectDependency(typeof(AbstractService), ServiceLifetime.Singleton)]
    public sealed class ChildService : AbstractService
    {
        public override Guid GetAbstractGuid { get; } = Guid.NewGuid();
    }

    /*********************/
    /* Self Service      */
    /*********************/

    [InjectDependency(typeof(SelfService), ServiceLifetime.Singleton)]
    public sealed class SelfService
    {
        public Guid GetSelfGuid { get; } = Guid.NewGuid();
    }
}