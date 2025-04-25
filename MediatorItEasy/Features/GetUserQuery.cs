using MediatorItEasy.Dtos;
using MediatorItEasy.Engine;

namespace MediatorItEasy.Features
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public int Id { get; set; }
    }
}
