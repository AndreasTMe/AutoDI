using AutoDI.Extensions;

using ExampleWebAPI;

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

app.Run();