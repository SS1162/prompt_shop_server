using Entities;
using Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace Tests
{
    
    namespace Tests
    {
        public class MainCategoriesIntegrationTests : IClassFixture<ShopIntegrationFixture>, IDisposable
        {
            private readonly ShopIntegrationFixture _fixture;
            private readonly MainCategoriesReposetory _repository;

            public MainCategoriesIntegrationTests(ShopIntegrationFixture fixture)
            {
                _fixture = fixture;
                // Hook: אתחול מחדש של ה-DB לפני כל טסט
                _fixture.CreateNewContext();
                _repository = new MainCategoriesReposetory(_fixture.Context);
            }

            // --- Happy Path Tests ---

            [Fact]
            public async Task GetMainCategories_WhenDataExists_ReturnsAllItems()
            {
                // Act
                var result = await _repository.GetMainCategoriesReposetoty();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count()); // יש 2 איברים ב-Seed
                Assert.Contains(result, m => m.MainCategoryName == "מוצרי חשמל");
            }

            [Fact]
            public async Task AddMainCategory_ValidData_SavesToDatabase()
            {
                // Arrange
                var newCategory = new MainCategory
                {
                    MainCategoryId = 3,
                    MainCategoryName = "צעצועים",
                    MainCategoryPrompt = "Toys"
                };

                // Act
                var result = await _repository.AddMainCategoriesReposetoty(newCategory);

                // Assert
                var inDb = await _fixture.Context.MainCategories.FindAsync((short)3);
                Assert.NotNull(inDb);
                Assert.Equal("צעצועים", inDb.MainCategoryName);
            }

            [Fact]
            public async Task UpdateMainCategory_ExistingItem_UpdatesDataInDb()
            {
                // Arrange
                var categoryToUpdate = await _fixture.Context.MainCategories.FindAsync((short)1);
                _fixture.Context.Entry(categoryToUpdate).State = EntityState.Detached; // ניתוק למניעת Tracking conflict

                categoryToUpdate.MainCategoryName = "מוצרי חשמל מעודכן";

                // Act
                await _repository.UpdateMainCategoriesReposetoty(1, categoryToUpdate);

                // Assert
                var updated = await _fixture.Context.MainCategories.FindAsync((short)1);
                Assert.Equal("מוצרי חשמל מעודכן", updated.MainCategoryName);
            }

            [Fact]
            public async Task GetById_ExistingId_ReturnsCorrectCategory()
            {
                // Act
                var result = await _repository.GetByIdMainCategoriesReposetoty(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("מוצרי חשמל", result.MainCategoryName);
            }

            [Fact]
            public async Task DeleteMainCategory_HappyPath_RemovesFromDb()
            {
                // Act
                await _repository.DeleteMainCategoriesReposetoty(2);

                // Assert
                var deleted = await _fixture.Context.MainCategories.FindAsync((short)2);
                Assert.Null(deleted);
            }

            // --- Unhappy Path Tests ---

            [Fact]
            public async Task GetMainCategories_WhenNoData_ReturnsEmptyList()
            {
                // Arrange - ניקוי כל הנתונים בסדר היררכי הפוך

                // 1. מחיקת פריטי הזמנה (הם תלויים במוצרים)
                _fixture.Context.OrdersItems.RemoveRange(_fixture.Context.OrdersItems);

                // 2. מחיקת מוצרים (הם תלויים בקטגוריות)
                _fixture.Context.Products.RemoveRange(_fixture.Context.Products);

                // 3. מחיקת קטגוריות (הן תלויות בקטגוריות ראשיות)
                _fixture.Context.Categories.RemoveRange(_fixture.Context.Categories);

                // 4. מחיקת הקטגוריות הראשיות (השורש)
                _fixture.Context.MainCategories.RemoveRange(_fixture.Context.MainCategories);

                // שמירת השינויים בבת אחת
                await _fixture.Context.SaveChangesAsync();

                // Act
                var result = await _repository.GetMainCategoriesReposetoty();

                // Assert
                Assert.Empty(result);
            }

            [Fact]
            public async Task GetById_NonExistingId_ReturnsNull()
            {
                // Act
                var result = await _repository.GetByIdMainCategoriesReposetoty(999);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task AddMainCategory_DuplicateId_ThrowsException()
            {
                // Arrange
                var duplicate = new MainCategory { MainCategoryId = 1, MainCategoryName = "Duplicate" };

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await _repository.AddMainCategoriesReposetoty(duplicate);
                });
            }

            [Fact]
            public async Task DeleteMainCategory_HappyPath_RemovesSuccessfully()
            {
                // Arrange
                // נשתמש ב-ID שקיים ב-Seed ואין לו תלויות (כפי שה-Service היה מאשר)
                long idToDelete = 2;

                // Act
                await _repository.DeleteMainCategoriesReposetoty(idToDelete);

                // Assert
                // בדיקה טכנית: האם האובייקט נעלם מה-DB
                var deletedItem = await _fixture.Context.MainCategories.FindAsync((short)idToDelete);
                Assert.Null(deletedItem);
            }

            [Fact]
            public async Task DeleteMainCategory_WhenServiceAllowedButDbFails_ThrowsException()
            {
                // טסט זה נועד לוודא ששגיאות DB אמיתיות (כמו בעיית תקשורת או אילוץ לא צפוי) עוברות הלאה
                // Arrange
                long nonExistingId = 999;

                // Act & Assert
                // מכיוון שה-Remove מקבל null (כי ה-ID לא קיים), נצפה לשגיאה טכנית
                await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                {
                    await _repository.DeleteMainCategoriesReposetoty(nonExistingId);
                });
            }
            public void Dispose()
            {
                // Hook: ניקוי ה-DB בסיום כל טסט
                _fixture.Context.Database.EnsureDeleted();
            }
        }
    }
}
