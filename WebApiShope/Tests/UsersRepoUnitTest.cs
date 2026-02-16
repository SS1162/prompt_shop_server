using Xunit;
using Moq;
using FluentAssertions;
using Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Tests
{
    public class UserRepositoryUnitTests : IClassFixture<UserRepoFixture>
    {
        private readonly UserRepoFixture _fixture;
        private readonly IUsersReposetory _repository;

        public UserRepositoryUnitTests(UserRepoFixture fixture)
        {
            _fixture = fixture;
            _repository = new UsersReposetory(_fixture.MockContext.Object);
        }

        [Fact]
        public async Task AddNewUsersRepositories_HappyPath_ShouldReturnUserWithGeneratedId()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "new_user@example.com",
                FirstName = "Israel",
                LastName = "Israeli",
                Password = "Password123"
            };

            // הגדרת ה-Mock כך שיעדכן את ה-ID (סימולציה של Identity מה-DB)
            _fixture.MockSet.Setup(m => m.AddAsync(It.IsAny<User>(), default))
                .Callback<User, CancellationToken>((u, ct) => u.UserId = 100);

            // Act
            var result = await _repository.AddNewUsersRepositories(newUser);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(100); // וידוא שה-Repository החזיר את האובייקט המעודכן מה-DB

            _fixture.MockSet.Verify(m => m.AddAsync(It.IsAny<User>(), default), Times.Once());
            _fixture.MockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task AddNewUsersRepositories_DatabaseCriticalError_ShouldPropagateException()
        {
            // Arrange
            var newUser = new User { UserName = "error@test.com" };

            // דימוי שגיאה טכנית קריטית במסד הנתונים
            _fixture.MockContext.Setup(m => m.SaveChangesAsync(default))
                .ThrowsAsync(new DbUpdateException("Internal Database Error"));

            // Act
            Func<Task> act = async () => await _repository.AddNewUsersRepositories(newUser);

            // Assert
            // מוודאים שה-Repository לא תופס את השגיאה ב-Try-Catch ריק, אלא זורק אותה למעלה
            await act.Should().ThrowAsync<DbUpdateException>();
        }


        [Fact]
        public async Task GetByIDUsersRepositories_UserExists_ShouldReturnUser()
        {
            // Arrange
            short existingId = 1;

            // Act
            var result = await _repository.GetByIDUsersRepositories(existingId);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(existingId);
   
        }

        [Fact]
        public async Task GetByIDUsersRepositories_UserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            long nonExistingId = 999;

            // Act
            var result = await _repository.GetByIDUsersRepositories(nonExistingId);

            // Assert
            result.Should().BeNull();
        }

       


        [Fact]
        public async Task LoginUsersRepositories_ValidCredentials_ShouldReturnUser()
        {
            // Arrange - שם משתמש וסיסמה תקינים לחלוטין
            var loginDetails = new User { UserName = "Admin1", Password = "p1" };

            // Act
            var result = await _repository.LoginUsersRepositories(loginDetails);

            // Assert
            result.Should().NotBeNull();
            result.UserName.Should().Be("Admin1");
        }

        [Fact]
        public async Task LoginUsersRepositories_CorrectUserWrongPassword_ShouldReturnNull()
        {
            // Arrange - שם משתמש קיים, אבל סיסמה שגויה
            var loginDetails = new User { UserName = "Admin1", Password = "wrongPassword123" };

            // Act
            var result = await _repository.LoginUsersRepositories(loginDetails);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginUsersRepositories_WrongUserCorrectPassword_ShouldReturnNull()
        {
            // Arrange - שם משתמש שלא קיים, אבל סיסמה שבמקרה קיימת אצל מישהו אחר
            var loginDetails = new User { UserName = "NonExistentUser", Password = "p1" };

            // Act
            var result = await _repository.LoginUsersRepositories(loginDetails);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginUsersRepositories_BothIncorrect_ShouldReturnNull()
        {
            // Arrange - גם שם המשתמש וגם הסיסמה שגויים
            var loginDetails = new User { UserName = "FakeUser", Password = "FakePassword" };

            // Act
            var result = await _repository.LoginUsersRepositories(loginDetails);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateUsersRepositories_HappyPath_ShouldInvokeUpdateAndSaveChangesAsync()
        {
            // Arrange
            short userId = 1;
            var updatedUser = new User
            {
                UserId = userId,
                UserName = "UpdatedAdmin",
                FirstName = "NewName",
                Password = "NewPassword123"
            };

            // Act
            await _repository.UpdateUsersRepositories(userId, updatedUser);

            // Assert
            // 1. וידוא שבוצעה קריאה למתודת ה-Update של ה-DbSet
            _fixture.MockSet.Verify(m => m.Update(It.Is<User>(u => u.UserId == userId && u.UserName == "UpdatedAdmin")), Times.Once());

            // 2. וידוא שבוצעה קריאה ל-SaveChangesAsync (הגרסה האסינכרונית)
            _fixture.MockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateUsersRepositories_ConcurrencyError_ShouldPropagateException()
        {
            // Arrange
            short userId = 2;
            var userToUpdate = new User { UserId = userId, UserName = "UpdateFail" };

            // דימוי שגיאת Concurrency בזמן השמירה האסינכרונית
            _fixture.MockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            // Act
            Func<Task> act = async () => await _repository.UpdateUsersRepositories(userId, userToUpdate);

            // Assert
            // וידוא שהשגיאה מבעבעת למעלה
            await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task UpdateUsersRepositories_DatabaseFailure_ShouldPropagateException()
        {
            // Arrange
            short userId = 3;
            var userToUpdate = new User { UserId = userId, UserName = "DbFail" };

            // דימוי נפילה כללית של ה-DB
            _fixture.MockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException("Database connection failed"));

            // Act
            Func<Task> act = async () => await _repository.UpdateUsersRepositories(userId, userToUpdate);

            // Assert
            await act.Should().ThrowAsync<DbUpdateException>()
                     .WithMessage("Database connection failed");
        }
    }
}