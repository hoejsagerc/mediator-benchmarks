using BenchmarkDotNet.Attributes;
using Mediator.Shared.Models;
using Mediator.Shared.TestHandlers;
using Microsoft.Extensions.DependencyInjection;
using Mediator.Impl.NickChapsas;

namespace Mediator.Benchmarks;


[MemoryDiagnoser]
public class MediatorImplementationBenchmarks
{
    private readonly Guid _requestId = Guid.NewGuid();
    private readonly string _requestName = "TestName";
    private readonly int _requestAge = 30;


    // Official MediatR implementation
    private MediatR.IMediator _officialMediatr = default!;


    // Simple Mediator implementation
    private Mediator.Impl.Simple.Interfaces.ISender _simpleMediator = default!;


    // Nick Chapsas Mediator implementation
    private Mediator.Impl.NickChapsas.Interfaces.IMediator _nickChapsasMediator = default!;

    // Another simple Mediator implementation
    private Mediator.Impl.AnotherSimple.Interfaces.IMediator _anotherSimpleMediator = default!;


    [GlobalSetup]
    public void Setup()
    {
        // Set up official MediatR implementation
        {
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(OfficialMediatrRecordCommand).Assembly));
            var serviceProvider = services.BuildServiceProvider();
            _officialMediatr = serviceProvider.GetRequiredService<MediatR.IMediator>();
        }

        {
            // Set up Simple Mediator implementation
            var services = new ServiceCollection();
            services.AddSingleton<Mediator.Impl.Simple.Interfaces.ISender, Mediator.Impl.Simple.Sender>();
            services.AddTransient<Mediator.Impl.Simple.Interfaces.IRequestHandler<SimpleMediatorCommand, TestResponse>, SimpleMediatorHandler>();
            var serviceProvider = services.BuildServiceProvider();
            _simpleMediator = serviceProvider.GetRequiredService<Mediator.Impl.Simple.Interfaces.ISender>();
        }

        // Nick Chapsas Mediator Implementation
        {
            var services = new ServiceCollection();
            services.AddNickMediator(ServiceLifetime.Transient, typeof(NickChapsasMediatorHandler));
            var serviceProvider = services.BuildServiceProvider();
            _nickChapsasMediator = serviceProvider.GetRequiredService<Mediator.Impl.NickChapsas.Interfaces.IMediator>();
        }

        // Another simple Mediator implementation
        {
            var services = new ServiceCollection();
            services.AddSingleton<Mediator.Impl.AnotherSimple.Interfaces.IMediator, Mediator.Impl.AnotherSimple.Mediator>();
            services.AddTransient<Mediator.Impl.AnotherSimple.Interfaces.IRequestHandler<AnotherSimpleMediatorCommand, TestResponse>, AnotherSimpleMediatorCommandHandler>();
            var serviceProvider = services.BuildServiceProvider();
            _anotherSimpleMediator = serviceProvider.GetRequiredService<Mediator.Impl.AnotherSimple.Interfaces.IMediator>();
        }
    }

    [Benchmark(Baseline = true)]
    public async Task<TestResponse> OfficialMediatrRecord_Send()
    {
        var request = new OfficialMediatrRecordStructCommand(_requestId, _requestName, _requestAge);
        return await _officialMediatr.Send(request);
    }


    [Benchmark]
    public async Task<TestResponse> SimpleMediatr_Send()
    {
        return await _simpleMediator.Send(new SimpleMediatorCommand(_requestId, _requestName, _requestAge));
    }


    [Benchmark]
    public async Task<TestResponse> NickChapsasMediatr_Send()
    {
        return await _nickChapsasMediator.SendAsync(new NickChapsasMediatorCommand(_requestId, _requestName, _requestAge));
    }

    [Benchmark]
    public async Task<TestResponse> AnotherSimpleMediatr_Send()
    {
        return await _anotherSimpleMediator.Send<AnotherSimpleMediatorCommand, TestResponse>(new AnotherSimpleMediatorCommand(_requestId, _requestName, _requestAge));
    }
}
