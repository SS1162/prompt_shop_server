using Entities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests;
using Xunit;
using Repositories;
namespace Tests
{


    public class CategoryRepositoryTests : IClassFixture<CategoryRepoFixture>
    {
        private readonly CategoryRepoFixture _fixture;
        private readonly ICategoriesReposetory _repository;

        public CategoryRepositoryTests(CategoryRepoFixture fixture)
        {
            _fixture = fixture;
            // אתחול ה-Repository עם ה-MockContext מה-Fixture
            _repository = new CategoriesReposetory(_fixture.MockContext.Object);
        }

        [Fact]
        public async Task GetCategories_HappyPath_ReturnsFilteredItems()
        {
            // Arrange
            long mainCategoryId = 1;
            int pageNumber = 1;
            int pageSize = 10;
            string search = "מחשבים";

            // Act
            var (items, totalCount) = await _repository.GetCategoriesReposetory(pageNumber, mainCategoryId, pageSize, search);

            // Assert
            Assert.NotNull(items);
            Assert.NotEmpty(items);
            Assert.Equal(1, totalCount); // ב-Fixture יש רק קטגוריה אחת עם "מחשבים"
            Assert.Contains("מחשבים", items.First().CategoryName);
        }

        [Fact]
        public async Task GetCategories_WhenSearchIsNull_ReturnsAllInMainCategory()
        {
            // Arrange
            long mainCategoryId = 1;
            int pageNumber = 1;
            int pageSize = 10;
            string? search = null; // בדיקת המקרה שבו אין חיפוש

            // Act
            var (items, totalCount) = await _repository.GetCategoriesReposetory(pageNumber, mainCategoryId, pageSize, search);

            // Assert
            Assert.Equal(2, totalCount); // ב-Fixture יש 2 קטגוריות תחת MainCategoryId 1
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetCategories_WhenPageSizeIsSmall_AppliesPaging()
        {
            // Arrange
            long mainCategoryId = 1;
            int pageNumber = 2; // דף שני
            int pageSize = 1;   // איבר אחד בדף
            string? search = null;

            // Act
            var (items, totalCount) = await _repository.GetCategoriesReposetory(pageNumber, mainCategoryId, pageSize, search);

            // Assert
            Assert.Single(items); // אמור לחזור רק איבר אחד בגלל ה-pageSize
            Assert.Equal(2, totalCount); // סך הכל קיימים 2, אבל חזר רק אחד
        }

        // --- GetByID Tests ---

        [Fact]
        public async Task GetByID_HappyPath_ReturnsCorrectCategory()
        {
            // Arrange
            long existingId = 1;

            // Act
            var result = await _repository.GetByIDCategoriesReposetory(existingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingId, result.CategoryId);
            Assert.Equal("מחשבים ניידים", result.CategoryName); // לוודא שזה האיבר הנכון מה-Fixture
        }

        [Fact]
        public async Task GetByID_WhenIdDoesNotExist_ReturnsNull()
        {
            // Arrange
            long nonExistingId = 999;

            // Act
            var result = await _repository.GetByIDCategoriesReposetory(nonExistingId);

            // Assert
            // לפי המימוש של FirstOrDefaultAsync, אם לא נמצא איבר הוא מחזיר null
            Assert.Null(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByID_InvalidId_ReturnsNull(long invalidId)
        {
            // Arrange & Act
            var result = await _repository.GetByIDCategoriesReposetory(invalidId);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task UpdateCategory_HappyPath_ShouldUpdateExistingCategory()
        {
            // Arrange
            // ניקח איבר קיים מה-Fixture ונשנה לו את השם
            var categoryToUpdate = _fixture.CategoriesList.First();
            categoryToUpdate.CategoryName = "Updated Name";

            // Act
            await _repository.UpdateCategoriesReposetory(categoryToUpdate.CategoryId, categoryToUpdate);

            // Assert
            // 1. בדיקה שהשם באמת השתנה ברשימה שב-Fixture
            var updatedInList = _fixture.CategoriesList.FirstOrDefault(c => c.CategoryId == categoryToUpdate.CategoryId);
            Assert.Equal("Updated Name", updatedInList.CategoryName);

            // 2. בדיקה שבוצעה קריאה ל-SaveChangesAsync
            _fixture.MockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        // --- Add Categories Tests ---

        [Fact]
        public async Task AddCategory_HappyPath_ShouldAddCategoryToList()
        {
            // Arrange
            var newCategory = new Category
            {
                CategoryId = 10,
                CategoryName = "New Electronics",
                MainCategoryId = 1
            };

            // Act
            var result = await _repository.AddCategoriesReposetory(newCategory);

            // Assert
            // 1. בדיקה שהפונקציה החזירה את האובייקט שהוספנו
            Assert.NotNull(result);
            Assert.Equal(newCategory.CategoryName, result.CategoryName);

            // 2. בדיקה שהאובייקט באמת נוסף לרשימה ב-Fixture (בזכות ה-Callback שהגדרנו)
            Assert.Contains(_fixture.CategoriesList, c => c.CategoryId == 10);

            // 3. אימות שבוצעה פקודת שמירה אחת בדיוק
            _fixture.MockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task AddCategory_WhenDatabaseFails_ShouldThrowException()
        {
            // Arrange
            var category = new Category { CategoryId = 11, CategoryName = "Fail Case" };

            // הגדרת המוק שיזרוק שגיאה בזמן השמירה
            _fixture.MockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database Save Failed"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(async () =>
                await _repository.AddCategoriesReposetory(category)
            );
        }


        [Fact]
        public async Task DeleteCategory_HappyPath_CallsRemoveAndSave()
        {
            // Arrange
            long categoryIdToDelete = 1;

            // Act
            await _repository.DeleteIDCategoriesReposetory(categoryIdToDelete);

            // Assert
            // 1. בדיקה שבוצעה קריאה ל-Remove עם ישות כלשהי מסוג Category
            _fixture.MockContext.Verify(m => m.Categories.Remove(It.IsAny<Category>()), Times.Once());

            // 2. אימות שבוצעה פקודת שמירה ל-DB
            _fixture.MockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

       
    }
}
