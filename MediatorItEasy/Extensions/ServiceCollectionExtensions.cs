using MediatorItEasy.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorItEasy.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void InstallServices(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Program).Assembly.ExportedTypes
                .Where(e => typeof(IInstaller).IsAssignableFrom(e) && !e.IsInterface && !e.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}
