using Entities;
using Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
  

    namespace Tests
    {
        public class CategoriesIntegrationTests : IClassFixture<ShopIntegrationFixture>, IDisposable
        {
            private readonly ShopIntegrationFixture _fixture;
            private readonly CategoriesReposetory _repository;

            public CategoriesIntegrationTests(ShopIntegrationFixture fixture)
            {
                _fixture = fixture;
                // HOOK: ניקוי ובנייה מחדש לפני כל טסט בודד
                _fixture.CreateNewContext();
                _repository = new CategoriesReposetory(_fixture.Context);
            }

            // --- GetCategories Tests ---

            [Fact]
            public async Task GetCategories_HappyPath_ReturnsFilteredItems()
            {
                // Arrange
                long mainCategoryId = 1;
                string search = "מחשבים";

                // Act
                var (items, totalCount) = await _repository.GetCategoriesReposetory(1, mainCategoryId, 10, search);

                // Assert
                Assert.Single(items);
                Assert.Equal(1, totalCount);
                Assert.Contains("מחשבים", items.First().CategoryName);
            }

            [Fact]
            public async Task GetCategories_WhenSearchIsNull_ReturnsAllInMainCategory()
            {
                // Act
                var (items, totalCount) = await _repository.GetCategoriesReposetory(1, 1, 10, null);

                // Assert
                Assert.Equal(2, totalCount);
                Assert.Equal(2, items.Count());
            }

            [Fact]
            public async Task GetCategories_WhenPageSizeIsSmall_AppliesPaging()
            {
                // Act - דף 2, גודל דף 1 (אמור להביא את האיבר השני)
                var (items, totalCount) = await _repository.GetCategoriesReposetory(2, 1, 1, null);

                // Assert
                Assert.Single(items);
                Assert.Equal(2, totalCount);
            }

            // --- GetByID Tests ---

            [Fact]
            public async Task GetByID_HappyPath_ReturnsCorrectCategory()
            {
                // Act
                var result = await _repository.GetByIDCategoriesReposetory(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("מחשבים ניידים", result.CategoryName);
            }

            [Theory]
            [InlineData(999)]
            [InlineData(0)]
            [InlineData(-1)]
            public async Task GetByID_InvalidOrMissingId_ReturnsNull(long id)
            {
                // Act
                var result = await _repository.GetByIDCategoriesReposetory(id);

                // Assert
                Assert.Null(result);
            }

            // --- Update Tests ---

            [Fact]
            public async Task UpdateCategory_HappyPath_ShouldUpdateInDatabase()
            {
                // Arrange
                var categoryToUpdate = await _fixture.Context.Categories.FindAsync((short)1);
                _fixture.Context.Entry(categoryToUpdate).State = EntityState.Detached; // ניתוק מה-Tracker

                categoryToUpdate.CategoryName = "Updated Name";

                // Act
                await _repository.UpdateCategoriesReposetory(1, categoryToUpdate);

                // Assert
                var updatedInDb = await _fixture.Context.Categories.FindAsync((short)1);
                Assert.Equal("Updated Name", updatedInDb.CategoryName);
            }

            // --- Add Tests ---

            [Fact]
            public async Task AddCategory_HappyPath_ShouldSaveToDb()
            {
                // Arrange
                var newCategory = new Category
                {
                    CategoryId = 10,
                    CategoryName = "New Electronics",
                    MainCategoryId = 1,
                    CategoryPrompt = "Test"
                };

                // Act
                var result = await _repository.AddCategoriesReposetory(newCategory);

                // Assert
                var inDb = await _fixture.Context.Categories.FindAsync((short)10);
                Assert.NotNull(inDb);
                Assert.Equal("New Electronics", inDb.CategoryName);
            }

            [Fact]
            public async Task AddCategory_DuplicateId_ThrowsException()
            {
                // Arrange
                // יצירת אובייקט חדש עם אותו ID שכבר קיים ב-Seed (למשל ID 1)
                var duplicate = new Category
                {
                    CategoryId = 1,
                    CategoryName = "Duplicate Name",
                    MainCategoryId = 1,
                    CategoryPrompt = "Test"
                };

                // Act & Assert
                // אנחנו מצפים ל-InvalidOperationException כי ה-Tracker יזהה כפל
                // אם זה עדיין נופל, נסי להחליף ל-DbUpdateException במידה והשגיאה מגיעה מה-DB
                await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await _repository.AddCategoriesReposetory(duplicate);
                });
            }

            // --- Delete Tests (תוספת חשובה לאינטגרציה) ---

            [Fact]
            public async Task DeleteCategory_WithNoProducts_RemovesFromDb()
            {
                // Arrange
                var emptyCategory = new Category
                {
                    CategoryId = 100,
                    MainCategoryId = 1,
                    CategoryName = "Empty Category",
                    CategoryPrompt = "Test"
                };
                await _fixture.Context.Categories.AddAsync(emptyCategory);
                await _fixture.Context.SaveChangesAsync();

                // Act
                await _repository.DeleteIDCategoriesReposetory(100);

                // Assert
                var deleted = await _fixture.Context.Categories.FindAsync((short)100);
                Assert.Null(deleted);
            }

            public void Dispose()
            {
                // Hook: ניקוי ה-DB בסיום כל טסט
                _fixture.Context.Database.EnsureDeleted();
            }
        }
    }
}
