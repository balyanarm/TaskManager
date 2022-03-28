using System.Threading.Tasks;

namespace Domain.Services.Users
{
    public class DataInitializer
    {
        public static async Task SeedUserAsync(IUsersService usersService, CreateUserDto defaultUserDto)
        {
            await usersService.CreateUserAsync(defaultUserDto);
        }
    }
}