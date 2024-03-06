using AutoDI.Attributes;
using AutoDI.Extensions;

using ExampleWebAPI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AutoInject();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/singleton", ([FromServices] ISingletonService service) => service.GetSingletonGuid());
app.MapGet("/scoped", ([FromServices] IScopedService service) => service.GetScopedGuid());
app.MapGet("/transient", ([FromServices] ITransientService service) => service.GetTransientGuid());
app.MapGet("/abstract", ([FromServices] AbstractService service) => service.GetAbstractGuid());
app.MapGet("/self", ([FromServices] SelfService service) => service.GetSelfGuid());

app.Run();

namespace ExampleWebAPI
{
    /*********************/
    /* Singleton Service */
    /*********************/

    public interface ISingletonService
    {
        Guid GetSingletonGuid();
    }

    [RegisterService<ISingletonService>(ServiceLifetime.Singleton)]
    public sealed class SingletonService : ISingletonService
    {
        public Guid GetSingletonGuid() => Guid.NewGuid();
    }

    /*********************/
    /* Scoped Service    */
    /*********************/

    public interface IScopedService
    {
        Guid GetScopedGuid();
    }

    [RegisterService<IScopedService>(ServiceLifetime.Scoped)]
    public sealed class ScopedService : IScopedService
    {
        public Guid GetScopedGuid() => Guid.NewGuid();
    }

    /*********************/
    /* Transient Service */
    /*********************/

    public interface ITransientService
    {
        Guid GetTransientGuid();
    }

    [RegisterService<ITransientService>(ServiceLifetime.Transient)]
    public sealed class TransientService : ITransientService
    {
        public Guid GetTransientGuid() => Guid.NewGuid();
    }

    /*********************/
    /* Abstract Service  */
    /*********************/

    public abstract class AbstractService
    {
        public abstract Guid GetAbstractGuid();
    }

    [RegisterService<AbstractService>(ServiceLifetime.Singleton)]
    public sealed class ChildService : AbstractService
    {
        public override Guid GetAbstractGuid() => Guid.NewGuid();
    }

    /*********************/
    /* Self Service      */
    /*********************/

    [RegisterService<SelfService>(ServiceLifetime.Singleton)]
    public sealed class SelfService
    {
        public Guid GetSelfGuid() => Guid.NewGuid();
    }
}