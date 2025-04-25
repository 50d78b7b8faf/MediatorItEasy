using MediatorItEasy.Dtos;
using MediatorItEasy.Engine;
using MediatorItEasy.Repositories;

namespace MediatorItEasy.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IMediator _mediator;

        public UserRepository(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<UserDto> GetUserById(int? id)
        {
            return Task.FromResult(new UserDto
            {
                Id = id,
                Name = "Towers"
            });
        }
    }
}