# TRDependencyInjection

Facilita a injeção de depedências através de anotações nas classes.

* TransientInjection: Registra a classe como __Transient__, ou seja, uma nova instância é criada a cada injeção;
* ScopedInjection: Registra a classe como __Scoped__, ou seja, uma nova instância é criada por requisição, ou escopo, utilizando o __IServiceScopeFactory.Create()__;
* SingletonInjection: Registra a classe como __Singleton__. ou seja, uma única instância é criada para todo o tempo de vida da aplicação.

### Como usar

Anote sua classe com uma das anotações disponíveis.

```c#
[ScopedInjection(typeof(IOrderRepository))]
class OrderRepository : IOrderRepository
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