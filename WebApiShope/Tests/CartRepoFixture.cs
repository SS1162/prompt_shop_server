using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Tests
{

    public class CartRepoFixture : IDisposable
    {
        // רשימת הנתונים שתשמש לשליפות (Get)
        public List<CartItem> CartsList { get; private set; }

        // ה-Mock של ה-Context
        public Mock<MyShop330683525Context> MockContext { get; private set; }

        public CartRepoFixture()
        {
            // 1. נתונים התחלתיים לדוגמה
            CartsList = new List<CartItem>
        {
            new CartItem
            {
                CartId = 1,
                UserId = 101,
                ProductsId = 50,
                //UserDescription = "Standard shipping",
                Valid = 1
            },
            new CartItem
            {
                CartId = 2,
                UserId = 102,
                ProductsId = 51,
                //UserDescription = "Gift package",
                Valid = 1
            }
        };

            // 2. יצירת ה-Mock ל-Context
            MockContext = new Mock<MyShop330683525Context>();

            // 3. הגדרת ה-DbSet עבור שליפות (שימוש ב-ReturnsDbSet מהספרייה)
            MockContext.Setup(c => c.CartItems).ReturnsDbSet(CartsList);

        
        }

        public void Dispose()
        {
            // ניקוי המעקב אחרי קריאות בין טסט לטסט
            MockContext.Invocations.Clear();
        }
    }
}
