using System.Threading.Tasks;
using Domain.Core;

namespace Domain.Services.Roles
{
    public interface IRolesService
    {
        Task<RoleDto> GetRoleByNameAsync(string name);

        Task<UserRoleDto> AddUserRoleAsync(UserRoleDto userRoleDto);

        Task<RoleDto> AddRoleAsync(Role role);
    }
}
