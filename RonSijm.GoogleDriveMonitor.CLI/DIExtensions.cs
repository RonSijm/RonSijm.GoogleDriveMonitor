using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RonSijm.GoogleDriveMonitor.CLI;

public static class DIExtensions
{
    public static void AddTypesAndInterfaces(this IServiceCollection services, Type targetType, params Type[] exclusions)
    {
        var types = targetType.Assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsGenericType).ToList();

        foreach (var type in types)
        {
            var lifetime = ServiceLifetime.Transient;

            if (exclusions != null)
            {
                if (exclusions.Any(x => x.IsAssignableFrom(type)))
                {
                    continue;
                }
            }

            services.Add(new ServiceDescriptor(type, type, lifetime));

            var interfaces = type.GetInterfaces();

            foreach (var typeInterface in interfaces)
            {
                // Skipping hosted Services to manually assign them, and start them in the correct order.
                if (typeInterface.IsAssignableFrom(typeof(IHostedService)))
                {
                    continue;
                }

                services.Add(new ServiceDescriptor(typeInterface, type, lifetime));
            }
        }
    }
}