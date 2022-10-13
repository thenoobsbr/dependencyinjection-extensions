
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=thenoobsbr_dependencyinjection-extensions&metric=coverage)](https://sonarcloud.io/dashboard?id=thenoobsbr_dependencyinjection-extensions)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=thenoobsbr_dependencyinjection-extensions&metric=alert_status)](https://sonarcloud.io/dashboard?id=thenoobsbr_dependencyinjection-extensions)
[![NuGet](https://buildstats.info/nuget/TheNoobs.DependencyInjection.Extensions.Modules.Abstractions)](http://www.nuget.org/packages/TheNoobs.DependencyInjection.Extensions.Modules.Abstractions)
[![NuGet](https://buildstats.info/nuget/TheNoobs.DependencyInjection.Extensions.Modules)](http://www.nuget.org/packages/TheNoobs.DependencyInjection.Extensions.Modules)
[![NuGet](https://buildstats.info/nuget/TheNoobs.DependencyInjection.Extensions.Attributes.Abstractions)](http://www.nuget.org/packages/TheNoobs.DependencyInjection.Extensions.Attributes.Abstractions)
[![NuGet](https://buildstats.info/nuget/TheNoobs.DependencyInjection.Extensions.Attributes)](http://www.nuget.org/packages/TheNoobs.DependencyInjection.Extensions.Attributes)
 
 # TheNoobs.DependencyInjection.Extensions

Facilita a injeção de depedências através de anotações nas classes.

* TransientInjection: Registra a classe como __Transient__, ou seja, uma nova instância é criada a cada injeção;
* ScopedInjection: Registra a classe como __Scoped__, ou seja, uma nova instância é criada por requisição, ou escopo, utilizando o __IServiceScopeFactory.Create()__;
* SingletonInjection: Registra a classe como __Singleton__. ou seja, uma única instância é criada para todo o tempo de vida da aplicação.

### Como usar

Anote sua classe com uma das anotações disponíveis.

```c#
[ScopedInjection(typeof(IOrderRepository), typeof(IOrderRepositoryReadOnly))]
class OrderRepository : IOrderRepository, IOrderRepositoryReadOnly
{
}

[SingletonInjection(typeof(HttpClient))]
class CustomHttpClient : HttpClient
{
}

[TransientInjection(typeof(IDomainEventHandler<Order>))]
class OrderEventHandler: IDomainEventHandler<Order>
{
}

```

You can retrieve a instance of a registered type if you want.

```c#
[ScopedInjection(new [] { typeof(IUnitOfWork) }, typeof(CustomDbContext)]
class CustomDbContext : DbContext, IUnitOfWork
{
}
```

Durante a configuração dos serviçoes, chame o método __AddInjections__ fornecendo os assemblies que deseja que sejam analisados.
```c#
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddInjections(Assembly.GetExecutingAssembly());
    ...
}
```

E pronto, agora é só usar.
