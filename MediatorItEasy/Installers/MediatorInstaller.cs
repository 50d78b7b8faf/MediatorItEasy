using MediatorItEasy.Dtos;
using MediatorItEasy.Engine;
using MediatorItEasy.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorItEasy.Installers
{
    public class MediatorInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IRequestHandler<GetUserQuery, UserDto>, GetUserQueryHandler>();
            services.AddSingleton<IMediator, Mediator>();
        }
    }
}
