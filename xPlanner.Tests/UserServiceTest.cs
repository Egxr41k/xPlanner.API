using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using xPlanner.Auth;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;
using xPlanner.Services;

namespace xPlanner.Tests
{
    public class UserServiceTest
    {
        private IUserService userService;

        private int NotExistingUserId = 0;
        private int ExistingUserId = 1;

        public UserServiceTest()
        {
            var dbContext = InMemmoryDbContext.Create();
            var hasher = new PasswordHasher();
            dbContext.Users.Add(new User
            {
                Id = ExistingUserId,
                CreatedAt = DateTime.UtcNow,
                Email = "testEmail@test.com",
                Name = "testEmail@test.com", 
                Password = hasher.Generate("password")
            });
            dbContext.SaveChanges();

            var repository = new UserRepository(dbContext);
            userService = new UserService(repository, new PasswordHasher());
        }

        [Fact]
        public async Task GetAll_BeforeInitialization_ReturnsEmptyList()
        {
            // Arrange

            // Act

            // Assert

        }

        [Fact]
        public async Task GetById_GivenNotExisted_ReturnsNull()
        {
            // Arrange

            // Act
            MyProfileResponse result = await userService.GetUser(NotExistingUserId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Add_GivenNotExisted_ReturnsUser()
        {
            // Arrange

            // Act
            var result = await userService.CreateUser("testEmail@test.com", "password");
            ExistingUserId = result.Id;

            // Assert
            Assert.IsType<User>(result);
        }

        [Fact]
        public async Task Add_GivenExisted_ReturnsNull()
        {
            // Arrange

            // Act
            var result = await userService.CreateUser("testEmail@test.com", "password");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetById_GivenExisted_ReturnsUser()
        {
            // Arrange

            // Act
            MyProfileResponse? result = await userService.GetUser(ExistingUserId);

            // Assert
            Assert.IsType<MyProfileResponse>(result);
        }
    }
}
