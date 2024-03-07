using AutoDI.Attributes;
using AutoDI.Extensions;

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

    public interface ISingletonService
    {
        Guid GetSingletonGuid { get; }
    }

    [RegisterService(typeof(ISingletonService), ServiceLifetime.Singleton)]
    public sealed class SingletonService : ISingletonService
    {
        public Guid GetSingletonGuid { get; } = Guid.NewGuid();
    }

    /*********************/
    /* Scoped Service    */
    /*********************/

    public interface IScopedService
    {
        Guid GetScopedGuid { get; }
    }

    [RegisterService(typeof(IScopedService), ServiceLifetime.Scoped)]
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

    [RegisterService(typeof(ITransientService), ServiceLifetime.Transient)]
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

    [RegisterService(typeof(AbstractService), ServiceLifetime.Singleton)]
    public sealed class ChildService : AbstractService
    {
        public override Guid GetAbstractGuid { get; } = Guid.NewGuid();
    }

    /*********************/
    /* Self Service      */
    /*********************/

    [RegisterService(typeof(SelfService), ServiceLifetime.Singleton)]
    public sealed class SelfService
    {
        public Guid GetSelfGuid { get; } = Guid.NewGuid();
    }
}