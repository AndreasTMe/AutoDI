# AutoDI

[![Tests](https://github.com/andreastme/AutoDI/actions/workflows/tests.yml/badge.svg)](https://github.com/andreastme/AutoDI/actions/workflows/tests.yml)

## Attributes

- `IsDependencyAttribute`: Marks a type (class or interface) as a dependency.
- `InjectDependencyAttribute`: Connects a class implementation to an interface, abstract class or to itself.

## Simple Case

Let's assume we have the following interface and class files:

IMyService.cs

```csharp
using AutoDI.Attributes;

namespace MyNamespace.Services.Interfaces;

[IsDependency]
public interface IMyService
{
    void DoSomething();
}
```

MyService.cs

```csharp
using AutoDI.Attributes;
using MyNamespace.Services.Interfaces;

namespace MyNamespace.Services;

[InjectDependency(typeof(IMyService), DependencyLifetime.Singleton)]
public class MyService : IMyService
{
    public void DoSomething()
    {
        // Do something
    }
}
```

Assuming that the assembly is named `MyNamespace` The above will generate the following file:

AutoDIServiceCollectionExtensions.g.cs

```csharp
using Microsoft.Extensions.DependencyInjection;
using MyNamespace.Services;
using MyNamespace.Services.Interfaces;

namespace MyNamespace
{
    public static class AutoDI_ServiceCollectionExtensions
    {
        public static void AddAutoDI(this IServiceCollection services)
        {
            services.AddSingleton<IMyService, MyService>();
        }
    }
}
```

## Keyed Services

Let's assume we have the following interface and class files:

IMyService.cs

```csharp
using AutoDI.Attributes;

namespace MyNamespace.Services.Interfaces;

[IsDependency]
public interface IMyService
{
    void DoSomething();
}
```

MyFirstService.cs

```csharp
using AutoDI.Attributes;
using MyNamespace.Services.Interfaces;

namespace MyNamespace.Services;

[InjectDependency(typeof(IMyService), DependencyLifetime.Singleton, "key1")]
public class MyFirstService : IMyService
{
    public void DoSomething()
    {
        // Do something
    }
}
```

MySecondService.cs

```csharp
using AutoDI.Attributes;
using MyNamespace.Services.Interfaces;

namespace MyNamespace.Services;

[InjectDependency(typeof(IMyService), DependencyLifetime.Singleton, "key2")]
public class MySecondService : IMyService
{
    public void DoSomething()
    {
        // Do something
    }
}
```

Assuming that the assembly is named `MyNamespace` The above will generate the following file:

AutoDIServiceCollectionExtensions.g.cs

```csharp
using Microsoft.Extensions.DependencyInjection;
using MyNamespace.Services;
using MyNamespace.Services.Interfaces;

namespace MyNamespace
{
    public static class AutoDI_ServiceCollectionExtensions
    {
        public static void AddAutoDI(this IServiceCollection services)
        {
            services.AddKeyedSingleton<IMyService, MyFirstService>("key1");
            services.AddKeyedSingleton<IMyService, MySecondService>("key2");
        }
    }
}
```