using Domain.Core;
using Domain.Repos;
using Domain.Services.Roles;
using Domain.Services.Security;
using Domain.Services.Users;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace Domain.Tests
{
    public class UsersServiceUnitTests
    {
        
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = Configuration.InitConfiguration();
        }

        [Test]
        public async Task ShouldCreateUser()
        {
            // arrange
            var usersRepoMock = new Mock<IRepository<User>>();
            var rolesService = new Mock<IRolesService>();
            var encryptService = new Mock<IEncryptService>();
            var optionsMock = new Mock<IOptions<Configs>>().SetupAllProperties();
            User createdUser = null;
            usersRepoMock.Setup(x => x.AddAsync(It.IsAny<User>())).Callback<User>(user =>
            {
                createdUser = user;
                createdUser.Id = 1;
            }).Returns<User>(user => Task.FromResult(user));

            encryptService.Setup(x => x.Encrypt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("encrypted_password");

            rolesService.Setup(service => service.GetRoleByNameAsync(It.Is<string>(rn => rn == "Client")))
                .Returns(Task.FromResult(new RoleDto
                {
                    Id = 1,
                    Name = "Client"
                }));

            UserRoleDto createdUserRole = null;
            rolesService.Setup(service => service.AddUserRoleAsync(It.IsAny<UserRoleDto>()))
                .Callback<UserRoleDto>(role =>
                {
                    createdUserRole = role;
                })
                .Returns<UserRoleDto>(u => Task.FromResult(u));

            optionsMock.SetupGet(x => x.Value.Encryptkey).Returns(_configuration["Configs:Encryptkey"]);
            
            var usersService = new UsersService(usersRepoMock.Object, rolesService.Object, encryptService.Object, optionsMock.Object);

            // act
            await usersService.CreateUserAsync(new CreateUserDto
            {
                UserName = "test_user",
                Password = "test_password",
                RoleName = "Client"
            });

            // assert
            Assert.AreEqual("test_user", createdUser.UserName);
            Assert.AreEqual(1, createdUser.Id);
            Assert.AreEqual("encrypted_password", createdUser.PasswordHash);
            Assert.AreEqual(1, createdUserRole.UserId);
            Assert.AreEqual(1, createdUserRole.RoleId);       
        }
    }
}