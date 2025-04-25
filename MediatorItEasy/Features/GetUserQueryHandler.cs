using MediatorItEasy.Dtos;
using MediatorItEasy.Engine;
using MediatorItEasy.Repositories;

namespace MediatorItEasy.Features
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;   
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserById(request.Id); 
        }
    }
}
