using Mediator.Impl.Simple.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Impl.Simple;

public class Sender : ISender
{
    private readonly IServiceProvider _serviceProvider;

    public Sender(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var behaviors = _serviceProvider
            .GetServices<IPipelineBehavior<IRequest<TResponse>, TResponse>>()
            .ToList();


        if (!behaviors.Any())
        {
            return await HandleRequest(request, cancellationToken);
        }

        RequestHandlerDelegate<TResponse> handler = () =>
            HandleRequest(request, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var previousHandler = handler;

            handler = () => behavior.Handle(request, previousHandler, cancellationToken);
        }

        return await handler();
    }


    private async Task<TResponse> HandleRequest<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken)
    {
        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new InvalidOperationException(
                $"Handler for request type {request.GetType().Name} not found.");
        }

        var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest<object>, object>.Handle));
        if (method is null)
        {
            throw new InvalidOperationException(
                $"Method {nameof(IRequestHandler<IRequest<object>, object>.Handle)} not found on handler type {handlerType.Name}.");
        }

        return await (Task<TResponse>)method
            .Invoke(handler, new object[] { request, cancellationToken })!;
    }
}
