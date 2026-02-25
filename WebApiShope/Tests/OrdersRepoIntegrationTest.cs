using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
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
        public class OrdersIntegrationTests : IClassFixture<ShopIntegrationFixture>, IDisposable
        {
            private readonly ShopIntegrationFixture _fixture;
            private readonly OrdersReposetory _repository;
            private readonly CartsReposetory _cartsRepository;

            public OrdersIntegrationTests(ShopIntegrationFixture fixture)
            {
                _fixture = fixture;
                _fixture.CreateNewContext(); // הבטחת DB נקי

                // יצירת הרפוזיטורי האמיתי של הסל כי הוא נדרש להוספת הזמנה
                _cartsRepository = new CartsReposetory(_fixture.Context);
                _repository = new OrdersReposetory(_fixture.Context, _cartsRepository);
            }

            // --- GetOrderById Tests ---

            [Fact]
            public async Task GetOrderById_ExistingId_ReturnsOrder()
            {
                // Act
                var result = await _repository.GetOrderByIdReposetory(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.OrderId);
            }

            // --- AddOrder Tests (הטסט הכי חשוב באינטגרציה) ---

            [Fact]
            public async Task AddOrder_HappyPath_SavesOrderAndDeleteUserCart()
            {
                // Arrange
                short userId = 1;
                // 1. נוסיף פריט לסל כדי שנוכל לוודא שהוא נמחק אחרי ההזמנה
                await _cartsRepository.CreateUserCartReposetory(new CartItem { UserId = userId, ProductsId = 1, BasicSitesPlatforms = 1 });

                var newOrder = new Order
                {
                    UserId = userId,
                    OrderSum = 500,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    BasicId = 1
                };

                // Act
                var result = await _repository.AddOrderReposetory(newOrder);

                // Assert
                // 1. וידוא שההזמנה נשמרה ב-DB
                var orderInDb = await _fixture.Context.Orders.FindAsync(result.OrderId);
                Assert.NotNull(orderInDb);

                // 2. וידוא שהסל של המשתמש נמחק (לפי הלוגיקה ב-Repository)
                var cartItems = await _fixture.Context.CartItems.Where(ci => ci.UserId == userId).ToListAsync();
                Assert.Empty(cartItems);
            }

            // --- UpdateStatus Tests ---

            [Fact]
            public async Task UpdateStatus_ExistingOrder_UpdatesData()
            {
                // Arrange
                var order = await _fixture.Context.Orders.FindAsync((short)1);
                _fixture.Context.Entry(order).State = EntityState.Detached;
                order.OrderSum = 999.9;

                // Act
                await _repository.UpdateStatusReposetory(1, order);

                // Assert
                var updated = await _fixture.Context.Orders.FindAsync((short)1);
                Assert.Equal(999.9, updated.OrderSum);
            }

            // --- OrderItems Tests ---

            [Fact]
            public async Task GetOrderItems_ReturnsCorrectItems()
            {
                // Act
                var result = await _repository.GetOrderItemsReposetory(1);

                // Assert
                Assert.NotEmpty(result);
                Assert.All(result, item => Assert.Equal(1, item.OrderId));
            }

            // --- בדיקות לפונקציות החדשות ---

            [Fact]
            public async Task CheckIfHasPlatformByPlatformID_Existing_ReturnsItem()
            {
                // Arrange - מניחים שב-Seed הזמנה 1 מכילה פלטפורמה 1
                long platformId = 1;

                // Act
                var result = await _repository.CheckIfHasPlatformByPlatformID(platformId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(platformId, result.BasicSitesPlatforms);
            }

            [Fact]
            public async Task CheckIfHasProductByProductID_NonExisting_ReturnsNull()
            {
                // Act
                var result = await _repository.CheckIfHasProductByProductID(999);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task CheckIfHasProductByProductID_ProductExistsInOrders_ReturnsOrdersItem()
            {
                // Arrange
                // ב-Fixture הגדרנו שב-Seed יש OrdersItem עם ProductsId = 1
                long existingProductId = 1;

                // Act
                var result = await _repository.CheckIfHasProductByProductID(existingProductId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(existingProductId, result.ProductsId);
                // בדיקה נוספת לוודא שהוא שייך להזמנה הנכונה מה-Seed
                Assert.Equal(1, result.OrderId);
            }

            [Fact]
            public async Task CheckIfHasProductByProductID_ProductDoesNotExistInOrders_ReturnsNull()
            {
                // Arrange
                // מזהה מוצר שלא קיים ב-Seed של ה-OrdersItems
                long nonExistingProductId = 999;

                // Act
                var result = await _repository.CheckIfHasProductByProductID(nonExistingProductId);

                // Assert
                Assert.Null(result);
            }


            [Fact]
            public async Task CheckIfHasPlatformByPlatformID_PlatformDoesNotExistInOrders_ReturnsNull()
            {
                // Arrange
                // מזהה פלטפורמה שלא מופיע באף OrdersItem ב-Seed שלנו
                long nonExistingPlatformId = 555;

                // Act
                var result = await _repository.CheckIfHasPlatformByPlatformID(nonExistingPlatformId);

                // Assert
                // אנחנו מצפים לקבל Null כי הפלטפורמה הזו לא נמצאת באף פריט הזמנה
                Assert.Null(result);
            }
            public void Dispose()
            {
                _fixture.Context.Database.EnsureDeleted();
            }
        }
    }
}
