using Mediator.Impl.NickChapsas.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mediator.Impl.NickChapsas.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        params Type[] markers)
    {
        var handlerInfo = new Dictionary<Type, Type>();

        foreach (var marker in markers)
        {
            var assembly = marker.Assembly;
            var requests = GetClassesImplementingInterface(assembly, typeof(IRequest<>));
            var handlers = GetClassesImplementingInterface(assembly, typeof(IRequestHandler<,>));

            requests.ForEach(x =>
            {
                handlerInfo[x] = 
                    handlers.SingleOrDefault(xx => x == xx.GetInterface("IRequestHandler`2")!.GetGenericArguments()[0]);
            });

            var serviceDescriptors = handlers.Select(x => new ServiceDescriptor(x, x, lifetime));
            services.TryAdd(serviceDescriptors);
        }

        services.AddSingleton<IMediator>(x => new Mediator(x.GetRequiredService, handlerInfo));

        return services;
    }

    private static List<Type> GetClassesImplementingInterface(System.Reflection.Assembly assembly, Type typeToMatch)
    {
        return assembly.ExportedTypes
                        .Where(type =>
                        {
                            var genericInterfaceTypes = type.GetInterfaces().Where(x => x.IsGenericType);
                            var implementRequestType = genericInterfaceTypes
                                .Any(x => x.GetGenericTypeDefinition() == typeToMatch);
                            return !type.IsInterface && !type.IsAbstract && implementRequestType;
                        }).ToList();
    }
}
