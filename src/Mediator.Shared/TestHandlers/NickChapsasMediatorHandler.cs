using Mediator.Impl.NickChapsas.Interfaces;
using Mediator.Shared.Models;

namespace Mediator.Shared.TestHandlers;

public record struct NickChapsasMediatorCommand(Guid Id, string Name, int Age) : IRequest<TestResponse>;
public sealed class NickChapsasMediatorHandler : IRequestHandler<NickChapsasMediatorCommand, TestResponse>
{
    public Task<TestResponse> HandleAsync(NickChapsasMediatorCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResponse(request.Id, request.Name, request.Age));
    }
}
