using MediatorItEasy.Dtos;
using MediatorItEasy.Features;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace MediatorItEasy.Engine
{
    public static class HandlerCache
    {
        /// <summary>
        /// Método que retorna un diccionario de manejadores.
        /// </summary>
        /// <returns>Un diccionario que mapea tipos de solicitud a funciones que manejan esas solicitudes.</returns>
        public static ConcurrentDictionary<Type, Func<IServiceProvider, object, CancellationToken, Task<object>>> GetHandlers()
        {
            return new()
            {
                /// <summary>
                /// Mapeo del tipo de solicitud GetUserQuery a su manejador correspondiente.
                /// </summary>
                [typeof(GetUserQuery)] = async (sp, request, token) =>
                {
                    var handler = sp.GetRequiredService<IRequestHandler<GetUserQuery, UserDto>>();
                    return await handler.Handle((GetUserQuery)request, token);
                },
            };
        }
    }
}
