using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var repository = new UserRepository(dbContext);
            userService = new UserService(repository, new PasswordHasher());
        }

        [Fact]
        public async Task GetAllUsers_before_initialization_should_return_null()
        {
            // Arrange


            // Act
            
            // Aseert

        }

        [Fact]
        public async Task GetNotExistingUsers_Should_Reurns_Null()
        {
            // Arrange

            // Act
            MyProfileResponse result = await userService.GetUser(NotExistingUserId);

            // Aseert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetExistingUser_Should_Returns_User()
        {
            // Arrange

            // Act
            MyProfileResponse? result = await userService.GetUser(ExistingUserId);

            // Aseert
            Assert.IsType<MyProfileResponse>(result);
            // дизайн символика внешний вид персонала
        }
    }
}
