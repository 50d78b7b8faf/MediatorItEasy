using MediatorItEasy.Dtos;

namespace MediatorItEasy.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserById(int? id);
    }
}
