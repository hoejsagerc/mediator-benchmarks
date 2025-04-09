using Mediator.Impl.AnotherSimple.Interfaces;
using Mediator.Shared.Models;

namespace Mediator.Shared.TestHandlers;

public record struct AnotherSimpleMediatorCommand(Guid Id, string Name, int Age) : IRequest<TestResponse>;
public class AnotherSimpleMediatorCommandHandler : IRequestHandler<AnotherSimpleMediatorCommand, TestResponse>
{
    public Task<TestResponse> Handle(AnotherSimpleMediatorCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResponse(request.Id, request.Name, request.Age));
    }
}
