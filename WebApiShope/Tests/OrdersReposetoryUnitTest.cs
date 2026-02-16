using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Moq;
using Repositories;
using System.Threading.Tasks;
using Xunit;
namespace Tests
{
    

    public class OrderRepositoryTests : IClassFixture<OrderRepoFixture>
    {
        private readonly OrderRepoFixture _fixture;
        private readonly IOrdersReposetory _repository;
        private readonly Mock<ICartsReposetory> _mockCartsRepo;
        public OrderRepositoryTests(OrderRepoFixture fixture)
        {
            _fixture = fixture;
            _mockCartsRepo = new Mock<ICartsReposetory>();

            // הזרקת המוקים מתוך ה-Fixture היחיד שלנו
            _repository = new OrdersReposetory(
             _fixture.MockContext.Object,
             _mockCartsRepo.Object
         );
        }

        [Fact]
        // Happy Path: בדיקה שהפונקציה מחזירה את ההזמנה הנכונה כשה-ID קיים
        public async Task GetOrderById_HappyPath_ReturnsCorrectOrder()
        {
            // Arrange
            long existingId = 1;

            // Act
            var result = await _repository.GetOrderByIdReposetory(existingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingId, result.OrderId);
            Assert.Equal(150.5, result.OrderSum); // וידאו נתונים מה-Fixture
        }

        [Fact]
        // Unhappy Path: בדיקה שהפונקציה מחזירה Null כשה-ID לא קיים בבסיס הנתונים
        public async Task GetOrderById_WhenOrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            long nonExistingId = 999;

            // Act
            var result = await _repository.GetOrderByIdReposetory(nonExistingId);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        // Unhappy Path: בדיקת קצה עבור מזהים לא הגיוניים (אפס או שלילי)
        public async Task GetOrderById_InvalidId_ReturnsNull(long invalidId)
        {
            // Act
            var result = await _repository.GetOrderByIdReposetory(invalidId);

            // Assert
            Assert.Null(result);
        }

       
        [Fact]
        // Happy Path: בדיקה שהזמנה מתווספת והעגלה נמחקת לפי UserId
        public async Task AddOrder_HappyPath_ShouldExecuteWithUserId()
        {
            // Arrange
            var userId = (long)101;
            var newOrder = new Order
            {
                OrderId = 10,
                UserId = userId,
                OrderSum = 500
            };

            // Act
            var result = await _repository.AddOrderReposetory(newOrder);

            // Assert
            Assert.NotNull(result);

            // 1. בדיקה שההזמנה נוספה ונשמרה ב-DB
            _fixture.MockContext.Verify(c => c.Orders.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once());
            _fixture.MockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());

            // 2. בדיקה שבוצעה קריאה למחיקת העגלה עבור המשתמש הספציפי
            _mockCartsRepo.Verify(r => r.DeleteUserCartReposetory(userId), Times.Once());
        }

        [Fact]
        // Unhappy Path: בדיקה שאם השמירה נכשלת, לא מתבצעת מחיקת עגלה
        public async Task AddOrder_WhenSaveFails_ShouldNotCallDeleteCart()
        {
            // Arrange
            var order = new Order { OrderId = 11, UserId = 102 };

            // נדמה שגיאה בשמירה
            _fixture.MockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database Error"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(async () =>
                await _repository.AddOrderReposetory(order)
            );

            // וידאו שמחיקת העגלה לא נקראה בגלל הכישלון בשמירה
            _mockCartsRepo.Verify(r => r.DeleteUserCartReposetory(It.IsAny<long>()), Times.Never());
        }


        [Fact]
        // Happy Path: בדיקה שעדכון הסטטוס מתבצע ונשמר בהצלחה
        public async Task UpdateStatus_HappyPath_ShouldUpdateAndSave()
        {
            // Arrange
            long orderId = 1;
            var orderToUpdate = new Order
            {
                OrderId = orderId,
                UserId = 101,
                OrderSum = 200
            };

            // Act
            await _repository.UpdateStatusReposetory(orderId, orderToUpdate);

            // Assert
            // 1. וידאו שבוצעה פקודת Update על ה-DbSet של ההזמנות
            _fixture.MockContext.Verify(c => c.Orders.Update(It.IsAny<Order>()), Times.Once());

            // 2. וידאו שבוצעה שמירה בבסיס הנתונים
            _fixture.MockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        // Unhappy Path: בדיקה שאם השמירה נכשלת (למשל בעיית תקשורת), השגיאה נזרקת
        public async Task UpdateStatus_WhenDatabaseFails_ShouldThrowException()
        {
            // Arrange
            long orderId = 1;
            var order = new Order { OrderId = orderId };

            // הגדרת המוק שיזרוק שגיאה בזמן השמירה
            _fixture.MockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Update failed"));

            // Act & Assert
            // מוודאים שהשגיאה צפה למעלה כצפוי
            await Assert.ThrowsAsync<System.Exception>(async () =>
                await _repository.UpdateStatusReposetory(orderId, order)
            );
        }


        [Fact]
        // Happy Path: בדיקה שחוזרים הפריטים הנכונים עבור הזמנה קיימת
        public async Task GetOrderItems_HappyPath_ReturnsCorrectItemsForOrder()
        {
            // Arrange
            long existingOrderId = 1;

            // Act
            var result = await _repository.GetOrderItemsReposetory(existingOrderId);

            // Assert
            Assert.NotNull(result);

            // לפי הנתונים שהוספנו ל-Fixture, להזמנה 1 יש 2 פריטים
            var itemsList = result.ToList();
            Assert.Equal(2, itemsList.Count);

            // וידאו שכל פריט שחזר באמת שייך להזמנה מספר 1
            Assert.All(itemsList, item => Assert.Equal(existingOrderId, item.OrderId));

            // בדיקה אופציונלית של תוכן (למשל תיאור המשתמש של הפריט הראשון)
            Assert.Contains(itemsList, item => item.UserDescription == "High quality logo");
        }

        [Fact]
        // Unhappy Path: בדיקה שעבור מזהה הזמנה שאינו קיים, חוזרת רשימה ריקה
        public async Task GetOrderItems_WhenOrderIdNotFound_ReturnsEmptyList()
        {
            // Arrange
            long nonExistingOrderId = 999;

            // Act
            var result = await _repository.GetOrderItemsReposetory(nonExistingOrderId);

            // Assert
            // ב-Entity Framework, שאילתת Where שלא מוצאת כלום מחזירה אוסף ריק (ולא Null)
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        // Happy Path: בדיקה עבור הזמנה אחרת (הזמנה 2) כדי לוודא שהסינון דינמי
        public async Task GetOrderItems_DifferentOrder_ReturnsDifferentItems()
        {
            // Arrange
            long secondOrderId = 2;

            // Act
            var result = await _repository.GetOrderItemsReposetory(secondOrderId);

            // Assert
            Assert.NotNull(result);

            // לפי ה-Fixture, להזמנה 2 הגדרנו פריט אחד בלבד
            Assert.Single(result);
            Assert.Equal("SEO Optimization", result.First().UserDescription);
        }

        // --- Tests for CheckIfHasProductByProductID ---

        [Fact]
        public async Task CheckIfHasProductByProductID_ProductExists_ReturnsOrdersItem()
        {
            // Arrange
            // ב-Fixture הגדרת פריט עם ProductsId = 50
            long existingProductId = 50;

            // Act
            var result = await _repository.CheckIfHasProductByProductID(existingProductId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingProductId, result.ProductsId);
            Assert.Equal(1, result.OrderId); // וידוא שזה שייך להזמנה הנכונה מהרשימה
        }

        [Fact]
        public async Task CheckIfHasProductByProductID_ProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            long nonExistingProductId = 999;

            // Act
            var result = await _repository.CheckIfHasProductByProductID(nonExistingProductId);

            // Assert
            Assert.Null(result);
        }

        // --- Tests for CheckIfHasPlatformByPlatformID ---

        [Fact]
        public async Task CheckIfHasPlatformByPlatformID_PlatformExists_ReturnsOrdersItem()
        {
            // Arrange
            // ב-Fixture הגדרת פריטים עם BasicSitesPlatforms = 1
            long existingPlatformId = 1;

            // Act
            var result = await _repository.CheckIfHasPlatformByPlatformID(existingPlatformId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingPlatformId, result.BasicSitesPlatforms);
        }

        [Fact]
        public async Task CheckIfHasPlatformByPlatformID_PlatformDoesNotExist_ReturnsNull()
        {
            // Arrange
            long nonExistingPlatformId = 888;

            // Act
            var result = await _repository.CheckIfHasPlatformByPlatformID(nonExistingPlatformId);

            // Assert
            Assert.Null(result);
        }
    }
}
