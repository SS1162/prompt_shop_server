using Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class MainCategoryGetTests : IClassFixture<MainCategoryRepoFixture>
{
    private readonly MainCategoryRepoFixture _fixture;
    private readonly IMainCategoriesReposetory _repository;

    public MainCategoryGetTests(MainCategoryRepoFixture fixture)
    {
        _fixture = fixture;
        _repository = new MainCategoriesReposetory(_fixture.MockContext.Object);
    }

    // בדיקה 1: בדיקת Happy Path - כשיש נתונים בבסיס הנתונים
    [Fact]
    public async Task GetMainCategories_WhenDataExists_ShouldReturnAllItems()
    {
        // Act
        var result = await _repository.GetMainCategoriesReposetoty();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_fixture.MainCategoriesList.Count, result.Count()); // כמות תואמת ל-Fixture
        Assert.Contains(result, m => m.MainCategoryName == "מוצרי חשמל"); // וודא שנתון קיים אכן נמצא
        Assert.DoesNotContain(result, m => m.MainCategoryName == "לא קיים"); // וודא שנתון שלא הוגדר לא נמצא
    }

    // בדיקה 2: בדיקת Unhappy Path - כשבסיס הנתונים ריק
    [Fact]
    public async Task GetMainCategories_WhenNoData_ShouldReturnEmptyList()
    {
        // Arrange - יצירת מצב זמני של טבלה ריקה עבור טסט זה בלבד
        var emptyList = new List<MainCategory>();
        var emptyMockSet = emptyList.BuildMockDbSet();
        _fixture.MockContext.Setup(c => c.MainCategories).Returns(emptyMockSet.Object);

        // Act
        var result = await _repository.GetMainCategoriesReposetoty();

        // Assert
        Assert.NotNull(result); // מוודא שלא חוזר Null שעלול להפיל את ה-Client
        Assert.Empty(result);   // מוודא שהרשימה ריקה (Count = 0)
    }

    // --- Happy Path: הוספה תקינה של קטגוריה ---
    [Fact]
    public async Task AddMainCategoriesReposetoty_WhenValid_ShouldReturnAddedCategory()
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
        Assert.NotNull(result);
        Assert.Equal(newCategory.MainCategoryName, result.MainCategoryName);

        // לוודא שהאובייקט אכן נוסף לרשימה ב-Fixture (בזכות ה-Callback ב-Fixture)
        _fixture.MockSet.Verify(m => m.AddAsync(It.IsAny<MainCategory>(), It.IsAny<CancellationToken>()), Times.Once());

        // לוודא שבוצעה קריאה לשמירה (SaveChangesAsync)
        _fixture.MockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce());
    }

    // --- Unhappy Path: שגיאה טכנית בעת השמירה (Database Exception) ---
    [Fact]
    public async Task AddMainCategoriesReposetoty_WhenDatabaseThrowsException_ShouldThrowException()
    {
        // Arrange
        var newCategory = new MainCategory { MainCategoryId = 4, MainCategoryName = "בדיקת שגיאה" };

        // הגדרת ה-Mock שיזרוק שגיאה בעת הניסיון לשמור
        _fixture.MockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new System.Exception("Database connection error"));

        // Act & Assert
        // לוודא שה-Repository מעביר את השגיאה הלאה (או זורק אותה) ולא "בולע" אותה
        await Assert.ThrowsAsync<System.Exception>(async () =>
        {
            await _repository.AddMainCategoriesReposetoty(newCategory);
        });
    }

    // --- Happy Path: עדכון תקין של קטגוריה קיימת ---
    [Fact]
    public async Task UpdateMainCategoriesReposetoty_WhenValid_ShouldUpdateData()
    {
        // Arrange
        var categoryToUpdate = _fixture.MainCategoriesList[0]; // לקיחת אובייקט קיים מה-Fixture
        categoryToUpdate.MainCategoryName = "מוצרי חשמל מעודכן";

        // Act
        await _repository.UpdateMainCategoriesReposetoty(categoryToUpdate.MainCategoryId, categoryToUpdate);

        // Assert
        // בדיקה שהשם אכן השתנה ברשימה הפנימית של ה-Fixture
        Assert.Equal("מוצרי חשמל מעודכן", _fixture.MainCategoriesList[0].MainCategoryName);

        // וידוא שבוצעה קריאה לשמירה ב-Context
        _fixture.MockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce());
    }

    // --- Unhappy Path: שגיאה טכנית בזמן העדכון (Concurrency או DB Error) ---
    [Fact]
    public async Task UpdateMainCategoriesReposetoty_WhenDatabaseError_ShouldThrowException()
    {
        // Arrange
        var category = new MainCategory { MainCategoryId = 1, MainCategoryName = "בדיקת שגיאה" };

        // הגדרת ה-Mock שיזרוק שגיאה בעת השמירה
        _fixture.MockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateConcurrencyException());

        // Act & Assert
        // לוודא שהשגיאה מבעבעת למעלה ולא נבלעת ב-Repository
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
        {
            await _repository.UpdateMainCategoriesReposetoty(category.MainCategoryId, category);
        });
    }


    // --- Happy Path: שליפת קטגוריה קיימת לפי ID ---
    [Fact]
    public async Task GetByIdMainCategoriesReposetoty_WhenIdExists_ShouldReturnCorrectCategory()
    {
        // Arrange
        short existingId = 1; // קיים ב-Fixture שלנו (Admin1)

        // Act
        var result = await _repository.GetByIdMainCategoriesReposetoty(existingId);

        // Assert
        Assert.NotNull(result); // מוודא שחזר אובייקט
        Assert.Equal(existingId, result.MainCategoryId); // מוודא שזה המזהה הנכון
        Assert.Equal("מוצרי חשמל", result.MainCategoryName); // מוודא שהנתונים תואמים ל-Fixture
    }

    // --- Unhappy Path: שליפת מזהה שלא קיים במערכת ---
    [Fact]
    public async Task GetByIdMainCategoriesReposetoty_WhenIdDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        short nonExistentId = 999;

        // Act
        var result = await _repository.GetByIdMainCategoriesReposetoty(nonExistentId);

        // Assert
        Assert.Null(result); // ב-Entity Framework, FirstOrDefault מחזיר Null כשאין התאמה
    }

    [Fact]
    public async Task DeleteMainCategory_HappyPath_CallsRemoveAndSave()
    {
        // Arrange
        long idToDelete = 1;

        // Act
        await _repository.DeleteMainCategoriesReposetoty(idToDelete);

        // Assert
        // 1. מוודאים שה-Context קיבל הוראה למחוק (Remove) עם ישות כלשהי מסוג MainCategory
        _fixture.MockContext.Verify(m => m.Remove(It.IsAny<MainCategory>()), Times.Once());

        // 2. מוודאים שבוצעה פקודת שמירה (SaveChangesAsync)
        _fixture.MockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }



}
