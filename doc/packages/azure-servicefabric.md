# Microsoft Service Fabric

To install the integration install this [NuGet package](https://www.nuget.org/packages/LogMagic.Microsoft.Azure.ServiceFabric/).

This package provides two ways of integrating with Service Fabric - **enrichment** and **correlating proxies**.

## Encrichment

To configure enrichment use the following syntax:

```csharp
L.Config
  .EnrichWith.AzureServiceFabricContext(this.Context);
```

where `Context` is your current service's `ServiceContext`. Normally you would initialise logging during service creation, for example in a service's constructor, where context is available:

```csharp
public MyService(StatefulServiceContext context)
    : base(context)
{
  L.Config.ClearWriters();

  L.Config
    .WriteTo.Trace()
    .EnrichWith.AzureServiceFabricContext(this.Context);
}
```

In the example above logging is written to standard trace and enriched with Service Fabric properties.

During enrichment the following properties are injected:

- **ServiceFabric.ServiceName**. The address of the deployed service.
- **ServiceFabric.ServiceTypeName**. Service type as it appears in the manifest.
- **ServiceFabric.PartitionId**. Partition ID.
- **ServiceFabric.ApplicationName**. The address of the application this service is contained in.
- **ServiceFabric.ApplicationTypeName**. Type of the application this service is contained in.
- **ServiceFabric.NodeName**. Name of the node this service runs on.

For stateless services another property is injected:

- **ServiceFabric.InstanceId**. Service instance ID.

And for stateful service there is another extra property:

- **ServiceFabric.ReplicaId**. Replica ID.

An example of those property values:

![Sf Enrichment Example](sf-enrichment-example.png)

## Correlating proxies

Logging becomes more complicated when you want to track calls between services, actors or a mix of them. LogMagic includes a way to track operations between services called **correlating proxies**. This includes support for the built-in Service Remoting protocol out of the box, and you can easily add your own protocol based on source code.

The way LogMagic does this is by transferring two properties called _operationId_ and _operationParentId_ between service calls. _operationId_ value is captured before the call is issued to the remote service (if it's present) and the remote service does the following:

- Picks up the value of _operationId_.
- Creates a new call context by generating a new unique value for _operationId_.
- Sets context property _operationParentId_ to the old value of _operationId_.
- In addition to that, both client and server maintains exact values of all context properties on both sides.

In order for this to work, you need to set up a few things first. However this library is called Log**Magic** and I've tried to make it a magic.

### Reliable Services

#### Client

Usually with raw Service Fabric you would create a remoting proxy in your code by calling a following piece of code:

```csharp
IRemoteServiceInterface = ServiceProxy.Create<IRemoteServiceInterface>(serviceUri, ...);
```

In order to capture the call context all you have to do is change `ServiceProxy` to `CorrelatingServiceProxy`, that's it:

```csharp
IRemoteServiceInterface = CorrelatingServiceProxy.Create<IRemoteServiceInterface>(serviceUri, ...);
```

We've kept method signatures identical to the ones Service Fabric SDK has, therefore no of the parameters have to change.

#### Server

On the listener service you would normally set up remoting using the following code:

```csharp
protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
{
    return new[] { new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context)) };
}
```

To make it aware of the context the code looks even simpler:

```csharp
protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
{
    return new[] { this.CreateCorrelatingReplicaListener() };
}
```

or in case of a stateless service:

```csharp
protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
{
    return new[] { this.CreateCorrelatingInstanceListener() };
}
```

Both methods accept a boolean flag `switchOperationContext` which is false by default and specifies whether to generate a new _operation ID_ on incoming request.

### Reliable Actors

We're following the idential idea with Realiable actors. 

#### Client

```csharp

```

> todo