using Domain.Core;
using Domain.Repos;
using Domain.Services.Auth;
using Domain.Services.Roles;
using Domain.Services.Security;
using Domain.Services.Users;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Domain.Tests
{
    public class AuthServiceUnitTests
    {
        private TaskManagerDbContext _dbContext;
        private IConfiguration _configuration;

        [SetUp]
        public async Task Setup()
        {
            _configuration = Configuration.InitConfiguration();
            DbContextOptionsBuilder<TaskManagerDbContext> optionsBuilder = new DbContextOptionsBuilder<TaskManagerDbContext>();
            optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"]);

            var dbContext = new TaskManagerDbContext(optionsBuilder.Options);
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Database.MigrateAsync();

            _dbContext = dbContext;
            await dbContext.Database.BeginTransactionAsync();
        }

        [TearDown]
        public async Task Down()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

        [Test]
        public async Task ShouldLogin()
        {
            // arrange
            await _dbContext.Set<Role>().AddRangeAsync(new[]
            {
                new Role{ Name = "Client" },
                new Role{ Name = "Admin" },
            });
            await _dbContext.SaveChangesAsync();

            var rolesRepository = new EntityRepository<Role>(_dbContext);
            var userRolesRepository = new EntityRepository<UserRole>(_dbContext);
            var usersRepository = new EntityRepository<User>(_dbContext);
            var rolesService = new RolesService(rolesRepository, userRolesRepository);
            var encryptService = new EncryptService();
            var optionsMock = new Moq.Mock<IOptions<Configs>>();
            optionsMock.Setup(x => x.Value).Returns(new Configs()
            {
                JwtAudience = _configuration["Configs:JwtAudience"],
                JwtIssuer = _configuration["Configs:JwtIssuer"],
                JwtKey = _configuration["Configs:JwtKey"],
                JwtDurationInMinutes = int.Parse(_configuration["Configs:JwtDurationInMinutes"]),
                Encryptkey = _configuration["Configs:Encryptkey"]
            });

            var usersService = new UsersService(usersRepository, rolesService, encryptService, optionsMock.Object);
            //act
            await usersService.CreateUserAsync(new CreateUserDto { UserName = "test_user", Password = "password", RoleName = "Admin"});

            var authService = new AuthService(usersService, encryptService, optionsMock.Object);
            var result = await authService.LoginAsync(new LoginDto { Password = "password", UserName = "test_user" });
            
            //assert
        }
    }
}