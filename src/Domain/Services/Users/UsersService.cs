using System;
using System.Collections;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Repos;
using Domain.Services.Roles;
using Domain.Services.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Domain.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IRolesService _rolesService;
        private readonly IEncryptService _encryptService;
        private readonly Configs _configs;


        public UsersService(
            IRepository<User> usersRepository,
            IRolesService rolesService,
            IEncryptService encryptService,
            IOptions<Configs> configs)
        {
            _usersRepository = usersRepository;
            _rolesService = rolesService;
            _encryptService = encryptService;
            _configs = configs.Value;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                PasswordHash = _encryptService.Encrypt(userDto.Password, _configs.Encryptkey)
            };
            user = await _usersRepository.AddAsync(user);
            var role = await _rolesService.GetRoleByNameAsync(userDto.RoleName);

            await _rolesService.AddUserRoleAsync(new UserRoleDto { UserId = user.Id, RoleId = role.Id });

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
            };
        }

        public async Task<UserWithPasswordHashDto?> GetUserWithHashByNameAsync(string userName)
        {
            var user = await _usersRepository.Query.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null) return null;

            return new UserWithPasswordHashDto
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash
            };
        }
        
        public async Task<UserDto?> GetUserByNameAsync(string userName)
        {
            var user = await _usersRepository.Query.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null) return null;

            return new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }

        public async Task<UserDto?> DeleteUserAsync(string userName)
        {
            var user = await _usersRepository.Query.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return null;
            }

            await _usersRepository.RemoveAsync(user);
            return new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
            };
        }
    }
}
