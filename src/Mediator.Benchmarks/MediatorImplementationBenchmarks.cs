using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using Mediator.Shared.Models;
using Mediator.Shared.TestHandlers;
using Microsoft.Extensions.DependencyInjection;

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


    // add your mediator field here...



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
            services.AddTransient<Mediator.Impl.Simple.Interfaces.ISender, Mediator.Impl.Simple.Sender>();
            services.AddTransient<Mediator.Impl.Simple.Interfaces.IRequestHandler<SimpleMediatorCommand, TestResponse>, SimpleMediatorHandler>();
            var serviceProvider = services.BuildServiceProvider();
            _simpleMediator = serviceProvider.GetRequiredService<Mediator.Impl.Simple.Interfaces.ISender>();
        }


        // add service registration for <your mediator> MediatR
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

    // add your benchmark method for <your mediator> here...
}
