using System.Collections.Concurrent;
using System.Reflection;

namespace MediatorItEasy.Engine
{
    /// <summary>
    /// Clase que implementa la interfaz IMediator.
    /// </summary>
    public class Mediator : IMediator
    {
        /// <summary>
        /// Diccionario que mapea tipos de solicitud a funciones que manejan esas solicitudes.
        /// A modo de ejemplo:
        /// Type = el caso de uso a invocar => typeof(GetUserQuery)
        /// IServiceProvider = Interfaz para lograr acceder al manejador asociado => IRequestHandler<GetUserQuery, UserDto>
        /// object = Objeto que representa IRequest => IRequest<UserDto>
        /// Task<object> = Tarea que ejecuta el método asociado al caso de uso => Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Func<IServiceProvider, object, CancellationToken, Task<object>>> _handlers = HandlerCache.GetHandlers();

        /// <summary>
        /// Proveedor de servicios para resolver dependencias.
        /// </summary>
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Constructor que inicializa el proveedor de servicios.
        /// </summary>
        /// <param name="provider">El proveedor de servicios.</param>
        public Mediator(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Implementación del método Send utilizando un diccionario de manejadores.
        /// </summary>
        /// <typeparam name="TResponse">El tipo de respuesta esperada.</typeparam>
        /// <param name="request">La solicitud que se va a enviar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>Una tarea que produce la respuesta.</returns>
        public async Task<TResponse> SendWithDictionary<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            // Obtiene el tipo concreto de la solicitud (por ejemplo, typeof(GetUserQuery))
            // El operador ! asume que 'request' no es null.
            var requestType = request!.GetType();

            // Declara la variable que contendrá la función delegada para manejar esta solicitud.
            // Esta función está precompilada y mapea (IServiceProvider, object, CancellationToken) → Task<object>
            Func<IServiceProvider, object, CancellationToken, Task<object>> handlerDelegate;

            // Intenta obtener del diccionario el delegado correspondiente al tipo de solicitud.
            // Si no está registrado, lanza una excepción.
            if (!_handlers.TryGetValue(requestType, out handlerDelegate))
            {
                throw new InvalidOperationException($"No hay manejadores registrados para {requestType.Name}");
            }

            // Ejecuta la función delegada, pasando el proveedor de servicios, la solicitud y el token de cancelación.
            // El resultado es una Task<object> que debe contener el TResponse esperado.
            var result = await handlerDelegate(_provider, request, cancellationToken);

            // Convierte el resultado a TResponse y lo retorna.
            // Se asume que el handler produjo correctamente una instancia del tipo esperado.
            return (TResponse)result;

        }

        /// <summary>
        /// Implementación del método Send utilizando reflexión.
        /// </summary>
        /// <typeparam name="TResponse">El tipo de respuesta esperada.</typeparam>
        /// <param name="request">La solicitud que se va a enviar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>Una tarea que produce la respuesta.</returns>
        public async Task<TResponse> SendWithReflection<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            // Obtiene el tipo concreto de la solicitud (por ejemplo, typeof(GetUserQuery))
            Type? requestType = request.GetType();

            // Construye el tipo genérico IRequestHandler<requestType, TResponse>
            // Esto nos permite resolver el manejador adecuado en tiempo de ejecución.
            Type? handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

            // Obtiene una instancia del manejador desde el contenedor de dependencias
            object? handler = _provider.GetService(handlerType);

            // Si no hay un manejador registrado, lanza una excepción
            if (handler == null)
                throw new InvalidOperationException($"No se tiene registrado un manejador para esta operación {handlerType.FullName}");

            // Obtiene la referencia al método 'Handle' del manejador
            MethodInfo? method = handlerType.GetMethod("Handle");

            // Si el método 'Handle' no existe, lanza una excepción
            if (method == null)
                throw new InvalidOperationException($"El manejador {handlerType.Name} no tiene implementado el método 'Handle'");

            // Invoca el método 'Handle' de forma reflexiva, pasando la request y el cancellationToken como argumentos
            // El resultado será un objeto que representa una tarea asincrónica (Task<TResponse>)
            object? result = method.Invoke(handler, new object[] { request, cancellationToken });

            // Verifica que el resultado es una tarea válida (Task)
            if (result is not Task task)
                throw new InvalidOperationException("El manejador no devolvió una tarea válida");

            // Espera a que la tarea finalice (sin importar si es Task o Task<T>)
            await task;

            // Si la tarea es del tipo esperado Task<TResponse>, devuelve el resultado
            if (task is Task<TResponse> typedTask)
                return typedTask.Result;

            // Si no es del tipo correcto, lanza una excepción indicando incompatibilidad de tipos
            throw new InvalidOperationException("La tarea no contiene el tipo de respuesta esperado");

        }
    }
}
