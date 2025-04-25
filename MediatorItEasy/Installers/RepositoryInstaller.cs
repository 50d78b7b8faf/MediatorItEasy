using MediatorItEasy.Repositories;
using MediatorItEasy.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorItEasy.Installers
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
        }
    }
}
