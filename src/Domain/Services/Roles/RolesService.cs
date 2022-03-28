using System;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Repos;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly IRepository<Role> _rolesRepository;
        private readonly IRepository<UserRole> _userRolesRepository;

        public RolesService(IRepository<Role> rolesRepository, IRepository<UserRole> userRolesRepository)
        {
            _rolesRepository = rolesRepository;
            _userRolesRepository = userRolesRepository;
        }

        public async Task<RoleDto> GetRoleByNameAsync(string name)
        {
            var role = await _rolesRepository.Query.FirstOrDefaultAsync(role => role.Name == name);
            if (role == null)
            {
                throw new Exception($"'{ name }' role not found.");
            }
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
            };
        }

        public async Task<RoleDto> AddRoleAsync(Role role)
        {
            var result = await _rolesRepository.AddAsync(role);
            return new RoleDto()
            {
                Id= result.Id,    
                Name = result.Name,
            };
        }
        
        public async Task<UserRoleDto> AddUserRoleAsync(UserRoleDto userRoleDto)
        {
            var userRole = new UserRole
            {
                RoleId = userRoleDto.RoleId,
                UserId = userRoleDto.UserId,
            };
            userRole = await _userRolesRepository.AddAsync(userRole);
            return new UserRoleDto
            {
                RoleId = userRole.RoleId,
                UserId = userRoleDto.UserId,
            };
        }
    }
}
