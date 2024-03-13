using System.Threading.Tasks;

using Xunit;

namespace AutoDI.SourceGen.Tests.Generators;

public class ServiceCollectionExtensionsGeneratorTests
{
    [Fact]
    public Task InjectDependency_InjectSingleton_ShouldGenerateAddSingleton()
    {
        const string input = @"using AutoDI.Attributes;

namespace Tests.Services;

[IsDependency]
public interface IService
{
    void DoSomething();
}

[InjectDependency(typeof(IService), DependencyLifetime.Singleton)]
public class Service : IService
{
    public void DoSomething()
    {
    }
}";

        return TestHelpers.Verify(input);
    }

    [Fact]
    public Task InjectDependency_InjectScoped_ShouldGenerateAddScoped()
    {
        const string input = @"using AutoDI.Attributes;

namespace Tests.Services;

[IsDependency]
public interface IService
{
    void DoSomething();
}

[InjectDependency(typeof(IService), DependencyLifetime.Scoped)]
public class Service : IService
{
    public void DoSomething()
    {
    }
}";

        return TestHelpers.Verify(input);
    }

    [Fact]
    public Task InjectDependency_InjectTransient_ShouldGenerateAddTransient()
    {
        const string input = @"using AutoDI.Attributes;

namespace Tests.Services;

[IsDependency]
public interface IService
{
    void DoSomething();
}

[InjectDependency(typeof(IService), DependencyLifetime.Transient)]
public class Service : IService
{
    public void DoSomething()
    {
    }
}";

        return TestHelpers.Verify(input);
    }
    
    [Fact]
    public Task InjectDependency_InjectSingletonAbstractClass_ShouldGenerateAddSingleton()
    {
        const string input = @"using AutoDI.Attributes;

namespace Tests.Services;

[IsDependency]
public abstract class AbstractService
{
    public abstract void DoSomething();
}

[InjectDependency(typeof(AbstractService), DependencyLifetime.Singleton)]
public class Service : AbstractService
{
    public override void DoSomething()
    {
    }
}";

        return TestHelpers.Verify(input);
    }
    
    [Fact]
    public Task InjectDependency_InjectSingletonClassAsSelf_ShouldGenerateAddSingleton()
    {
        const string input = @"using AutoDI.Attributes;

namespace Tests.Services;

[InjectDependency(typeof(Service), DependencyLifetime.Singleton)]
public class Service
{
    public void DoSomething()
    {
    }
}";

        return TestHelpers.Verify(input);
    }
    
    [Fact]
    public Task InjectDependency_InjectTwoKeyedSingletons_ShouldGenerateAddSingletonForTwoClasses()
    {
        const string input = @"using AutoDI.Attributes;

namespace Tests.Services;

[IsDependency]
public interface IService
{
    void DoSomething();
}

[InjectDependency(typeof(IService), DependencyLifetime.Singleton, ""1"")]
public class KeyedService1 : IService
{
    public void DoSomething()
    {
    }
}

[InjectDependency(typeof(IService), DependencyLifetime.Singleton, ""2"")]
public class KeyedService2 : IService
{
    public void DoSomething()
    {
    }
}";

        return TestHelpers.Verify(input);
    }
    
    [Fact]
    public Task InjectDependency_InjectTwoKeyedScoped_ShouldGenerateAddScopedForTwoClasses()
    {
        const string input = @"using AutoDI.Attributes;

namespace Tests.Services;

[IsDependency]
public interface IService
{
    void DoSomething();
}

[InjectDependency(typeof(IService), DependencyLifetime.Scoped, ""1"")]
public class KeyedService1 : IService
{
    public void DoSomething()
    {
    }
}

[InjectDependency(typeof(IService), DependencyLifetime.Scoped, ""2"")]
public class KeyedService2 : IService
{
    public void DoSomething()
    {
    }
}";

        return TestHelpers.Verify(input);
    }
    
    [Fact]
    public Task InjectDependency_InjectTwoKeyedTransients_ShouldGenerateAddTransientForTwoClasses()
    {
        const string input = @"using AutoDI.Attributes;

namespace Tests.Services;

[IsDependency]
public interface IService
{
    void DoSomething();
}

[InjectDependency(typeof(IService), DependencyLifetime.Transient, ""1"")]
public class KeyedService1 : IService
{
    public void DoSomething()
    {
    }
}

[InjectDependency(typeof(IService), DependencyLifetime.Transient, ""2"")]
public class KeyedService2 : IService
{
    public void DoSomething()
    {
    }
}";

        return TestHelpers.Verify(input);
    }
}