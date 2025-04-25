namespace MediatorItEasy.Engine
{
    /// <summary>
    /// Interfaz que define el contrato para el mediador.
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Método para enviar una solicitud utilizando un diccionario de manejadores.
        /// </summary>
        /// <typeparam name="TResponse">El tipo de respuesta esperada.</typeparam>
        /// <param name="request">La solicitud que se va a enviar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>Una tarea que produce la respuesta.</returns>
        Task<TResponse> SendWithDictionary<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Método para enviar una solicitud utilizando reflexión.
        /// </summary>
        /// <typeparam name="TResponse">El tipo de respuesta esperada.</typeparam>
        /// <param name="request">La solicitud que se va a enviar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>Una tarea que produce la respuesta.</returns>
        Task<TResponse> SendWithReflection<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
