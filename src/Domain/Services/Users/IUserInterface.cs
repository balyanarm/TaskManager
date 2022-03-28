using System.Threading.Tasks;
using Domain.Core;

namespace Domain.Services.Users
{
    public interface IUsersService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto?> GetUserByNameAsync(string userName);
        Task<UserWithPasswordHashDto?> GetUserWithHashByNameAsync(string userName);
        Task<UserDto?> DeleteUserAsync(string user);
    }
}