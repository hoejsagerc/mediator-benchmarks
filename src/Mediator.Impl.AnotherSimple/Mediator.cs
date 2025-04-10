using Microsoft.Extensions.DependencyInjection;
using Mediator.Impl.AnotherSimple.Interfaces;

namespace Mediator.Impl.AnotherSimple;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest<TResponse>
    {
        var handler = _serviceProvider.GetService<IRequestHandler<TRequest, TResponse>>();

        if (handler == null)
        {
            throw new Exception("No handler registered for the request type.");
        }

        return await handler.Handle(request, cancellationToken);
    }
}
