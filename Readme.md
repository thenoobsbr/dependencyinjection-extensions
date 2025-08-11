[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=thenoobsbr_dependencyinjection-extensions&metric=coverage)](https://sonarcloud.io/summary/new_code?id=thenoobsbr_dependencyinjection-extensions)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=thenoobsbr_dependencyinjection-extensions&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=thenoobsbr_dependencyinjection-extensions)

# TheNoobs.DependencyInjection.Extensions

A powerful and lightweight .NET library that provides modular dependency injection setup capabilities for ASP.NET Core applications. This library enables you to organize your dependency injection configuration into reusable modules with support for ordered execution.

## Features

- **Modular Architecture**: Organize your DI configuration into logical modules
- **Ordered Execution**: Control the order of module execution using `IOrderedModule`
- **Multiple Extension Points**: Support for host, service, and application-level configurations
- **Easy Integration**: Simple extension methods for `IHostApplicationBuilder` and `IApplicationBuilder`
- **Lightweight**: Minimal overhead with clean abstractions
- **Multi-Target**: Supports .NET 8.0 and .NET 9.0

## Installation

Install the packages via NuGet:

```bash
# Install the main package
dotnet add package TheNoobs.DependencyInjection.Extensions.Modules

# Install abstractions (if you need to reference interfaces in separate projects)
dotnet add package TheNoobs.DependencyInjection.Extensions.Modules.Abstractions
```

## Quick Start

### 1. Create Module Setup Classes

```csharp
using TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;

// Service registration module
public class DatabaseModuleSetup : IServiceModuleSetup
{
    public void Setup(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUserRepository, UserRepository>();
    }
}

// Application pipeline module
public class MiddlewareModuleSetup : IApplicationModuleSetup
{
    public void Setup(IApplicationBuilder appBuilder)
    {
        appBuilder.UseAuthentication();
        appBuilder.UseAuthorization();
    }
}

// Host configuration module
public class HostModuleSetup : IHostApplicationModule
{
    public void Setup(IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
    }
}
```

### 2. Register Modules in Your Application

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register all modules from the current assembly
builder.AddInjections(typeof(Program).Assembly);

var app = builder.Build();

// Configure application modules
app.UseInjections(typeof(Program).Assembly);

app.Run();
```

## Advanced Usage

### Ordered Module Execution

Control the execution order of your modules by implementing `IOrderedModule`:

```csharp
public class DatabaseModuleSetup : IServiceModuleSetup, IOrderedModule
{
    public int Order => 1; // Execute first
    
    public void Setup(IServiceCollection services, IConfiguration configuration)
    {
        // Database setup
    }
}

public class CachingModuleSetup : IServiceModuleSetup, IOrderedModule
{
    public int Order => 2; // Execute after database setup
    
    public void Setup(IServiceCollection services, IConfiguration configuration)
    {
        // Caching setup that depends on database
    }
}
```

### Multiple Assemblies

Register modules from multiple assemblies:

```csharp
var assemblies = new[]
{
    typeof(Program).Assembly,
    typeof(SomeOtherModule).Assembly,
    Assembly.LoadFrom("ExternalModule.dll")
};

builder.AddInjections(assemblies);
app.UseInjections(assemblies);
```

## API Reference

### Extension Methods

#### `AddInjections(params Assembly[] assemblies)`
Registers all modules found in the specified assemblies during the host building phase.

- Scans for classes implementing `IHostApplicationModule` and `IServiceModuleSetup`
- Executes modules in order (if `IOrderedModule` is implemented)
- Called on `IHostApplicationBuilder`

#### `UseInjections(params Assembly[] assemblies)`
Configures all application modules found in the specified assemblies during the application building phase.

- Scans for classes implementing `IApplicationModuleSetup`
- Executes modules in order (if `IOrderedModule` is implemented)
- Called on `IApplicationBuilder`

### Interfaces

#### `IServiceModuleSetup`
Interface for service registration modules.

```csharp
public interface IServiceModuleSetup
{
    void Setup(IServiceCollection services, IConfiguration configuration);
}
```

#### `IApplicationModuleSetup`
Interface for application pipeline configuration modules.

```csharp
public interface IApplicationModuleSetup
{
    void Setup(IApplicationBuilder appBuilder);
}
```

#### `IHostApplicationModule`
Interface for host-level configuration modules.

```csharp
public interface IHostApplicationModule
{
    void Setup(IHostApplicationBuilder builder);
}
```

#### `IOrderedModule`
Interface for controlling module execution order.

```csharp
public interface IOrderedModule
{
    int Order { get; }
}
```

## Examples

### Complete Web API Setup

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.AddInjections(typeof(Program).Assembly);

var app = builder.Build();

app.UseInjections(typeof(Program).Assembly);

app.MapControllers();
app.Run();

// Modules/ApiModuleSetup.cs
public class ApiModuleSetup : IServiceModuleSetup, IOrderedModule
{
    public int Order => 1;
    
    public void Setup(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}

// Modules/SwaggerModuleSetup.cs
public class SwaggerModuleSetup : IApplicationModuleSetup, IOrderedModule
{
    public int Order => 1;
    
    public void Setup(IApplicationBuilder appBuilder)
    {
        appBuilder.UseSwagger();
        appBuilder.UseSwaggerUI();
    }
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Links

- [GitHub Repository](https://github.com/thenoobsbr/dependency-injection-extensions)
- [NuGet Package - Main](https://www.nuget.org/packages/TheNoobs.DependencyInjection.Extensions.Modules)
- [NuGet Package - Abstractions](https://www.nuget.org/packages/TheNoobs.DependencyInjection.Extensions.Modules.Abstractions)
