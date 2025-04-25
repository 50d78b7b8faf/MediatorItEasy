namespace MediatorItEasy.Engine
{
    public interface IRequest<TResponse> { }

    public interface IRequestHandler<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
