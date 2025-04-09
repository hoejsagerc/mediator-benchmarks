using Mediator.Shared.Models;
using MediatR;

namespace Mediator.Shared.TestHandlers;



// Record performance in MediatR
public record OfficialMediatrRecordCommand(Guid Id, string Name, int Age) : IRequest<TestResponse>;

public sealed record OfficialMediatrRecordHandler
    : IRequestHandler<OfficialMediatrRecordCommand, TestResponse>
{
    public Task<TestResponse> Handle(OfficialMediatrRecordCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResponse(request.Id, request.Name, request.Age));
    }
}


// Structs performance in MediatR
public struct OfficialMediatrStructCommand : IRequest<TestResponse>
{
    public OfficialMediatrStructCommand(Guid id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    public Guid Id { get; }
    public string Name { get; }
    public int Age { get; }
}

public sealed class OfficialMediatrStructHandler
    : IRequestHandler<OfficialMediatrStructCommand, TestResponse>
{
    public Task<TestResponse> Handle(OfficialMediatrStructCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResponse(request.Id, request.Name, request.Age));
    }
}



public record struct OfficialMediatrRecordStructCommand(Guid Id, string Name, int Age) : IRequest<TestResponse>;

public sealed class OfficialMediatrRecordStructHandler
    : IRequestHandler<OfficialMediatrRecordStructCommand, TestResponse>
{
    public Task<TestResponse> Handle(OfficialMediatrRecordStructCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResponse(request.Id, request.Name, request.Age));
    }
}


public record struct OfficialMediatrClassCommand : IRequest<TestResponse>
{
    public OfficialMediatrClassCommand(Guid id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    public Guid Id { get; }
    public string Name { get; }
    public int Age { get; }
}

public sealed class OfficialMediatrClassHandler
    : IRequestHandler<OfficialMediatrClassCommand, TestResponse>
{
    public Task<TestResponse> Handle(OfficialMediatrClassCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResponse(request.Id, request.Name, request.Age));
    }
}