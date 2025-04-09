namespace BenchmarkDotNet;

using BenchmarkDotNet.Attributes;
using Mediator.Shared.Models;
using Mediator.Shared.TestHandlers;
using Microsoft.Extensions.DependencyInjection;




[MemoryDiagnoser]
public class MediatorCommandTypeBenchmarks
{
    private readonly Guid _requestId = Guid.NewGuid();
    private readonly string _requestName = "TestName";
    private readonly int _requestAge = 30;


    // Official MediatR implementation
    private MediatR.IMediator _officialMediatr = default!;


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
    }


    [Benchmark(Baseline = true)]
    public async Task<TestResponse> OfficialMediatrRecord_Send()
    {
        var request = new OfficialMediatrRecordCommand(_requestId, _requestName, _requestAge);
        return await _officialMediatr.Send(request);
    }


    [Benchmark]
    public async Task<TestResponse> OfficialMediatrStruct_Send()
    {
        var request = new OfficialMediatrStructCommand(_requestId, _requestName, _requestAge);
        return await _officialMediatr.Send(request);
    }


    [Benchmark]
    public async Task<TestResponse> OfficialMediatrRecordStruct_Send()
    {
        var request = new OfficialMediatrRecordStructCommand(_requestId, _requestName, _requestAge);
        return await _officialMediatr.Send(request);
    }

    [Benchmark]
    public async Task<TestResponse> OfficialMediatrClass_Send()
    {
        var request = new OfficialMediatrClassCommand(_requestId, _requestName, _requestAge);
        return await _officialMediatr.Send(request);
    }
}
