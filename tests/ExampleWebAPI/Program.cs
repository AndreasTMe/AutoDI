using ExampleWebAPI;
using ExampleWebAPI.Services;
using ExampleWebAPI.Services.Abstractions;
using ExampleWebAPI.Services.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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

app.MapGet("/keyed-singleton-1", ([FromKeyedServices("1")] IKeyedSingletonService service) => service.GetSingletonGuid);
app.MapGet("/keyed-singleton-2", ([FromKeyedServices("2")] IKeyedSingletonService service) => service.GetSingletonGuid);
app.MapGet("/keyed-scoped-1", ([FromKeyedServices("1")] IKeyedScopedService service) => service.GetScopedGuid);
app.MapGet("/keyed-scoped-2", ([FromKeyedServices("2")] IKeyedScopedService service) => service.GetScopedGuid);
app.MapGet("/keyed-transient-1", ([FromKeyedServices("1")] IKeyedTransientService service) => service.GetTransientGuid);
app.MapGet("/keyed-transient-2", ([FromKeyedServices("2")] IKeyedTransientService service) => service.GetTransientGuid);
app.MapGet("/keyed-abstract-1", ([FromKeyedServices("1")] KeyedAbstractService service) => service.GetAbstractGuid);
app.MapGet("/keyed-abstract-2", ([FromKeyedServices("2")] KeyedAbstractService service) => service.GetAbstractGuid);

app.Run();