using Mediator.Shared.Models;
using Mediator.Impl.Simple.Interfaces;

namespace Mediator.Shared.TestHandlers;

public record struct SimpleMediatorCommand(Guid Id, string Name, int Age) : IRequest<TestResponse>;

public sealed class SimpleMediatorHandler : IRequestHandler<SimpleMediatorCommand, TestResponse>
{
    public Task<TestResponse> Handle(SimpleMediatorCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResponse(request.Id, request.Name, request.Age));
    }
}