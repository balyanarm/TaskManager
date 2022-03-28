using Domain.Core;
using Domain.Repos;
using Domain.Services.Roles;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Domain.Tests
{
    public class RolesServiceUnitTests
    {
        private TaskManagerDbContext _dbContext;

        [SetUp]
        public async Task Setup()
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder();
            csBuilder.UserID = "sa";
            csBuilder.Password = "supeR_password1";
            csBuilder.DataSource = "localhost,1433";
            csBuilder.InitialCatalog = "taskManager";
            csBuilder.TrustServerCertificate = true;
            DbContextOptionsBuilder<TaskManagerDbContext> optionsBuilder = new DbContextOptionsBuilder<TaskManagerDbContext>();
            var connString = csBuilder.ToString();
            optionsBuilder.UseSqlServer(csBuilder.ToString());

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
        public async Task ShouldCreateUserRole()
        {
            // arrange
            await _dbContext.Set<Role>().AddRangeAsync(new[]
            {
                new Role{ Name = "Admin" },
            });
            var roleEntry = await _dbContext.Set<Role>().AddAsync(new Role { Name = "Client" });
            var userEntry = await _dbContext.Set<User>().AddAsync(new User { UserName = "test_user", PasswordHash = "password_hash" });
            await _dbContext.SaveChangesAsync();

            var rolesRepository = new EntityRepository<Role>(_dbContext);
            var userRolesRepository = new EntityRepository<UserRole>(_dbContext);

            var userRolesService = new RolesService(rolesRepository, userRolesRepository);

            // act
            var userRole = await userRolesService.AddUserRoleAsync(new UserRoleDto
            {
                RoleId = roleEntry.Entity.Id,
                UserId = userEntry.Entity.Id,
            });

            // assert
            var userRoleActual = await _dbContext.Set<UserRole>().FirstAsync(ur => ur.UserId == userEntry.Entity.Id);
            Assert.AreEqual(roleEntry.Entity.Id, userRoleActual.RoleId);
        }
        
    }
}