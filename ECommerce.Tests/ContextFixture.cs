using ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests
{
    public class ContextFixture : IDisposable
    {
        public Context context;

        public ContextFixture()
        {
            var dbName = "TestingDB" + DateTime.Now.ToFileTimeUtc();
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(dbName).Options;
            context = new Context(options);
            context.Database.EnsureDeleted();
            if (context.Products.Count() == 0)
            {
                context.Products.Add(new Models.Product { ProductId = 1, ProductName = "Banana", ProductQuantity = 30, ProductPrice = 3, ProductDescription = "Oooh banana!", ProductImage = "" });
                context.Products.Add(new Models.Product { ProductId = 2, ProductName = "Honeydew", ProductQuantity = 50, ProductPrice = 5, ProductDescription = "The best fruit", ProductImage = "" });
                context.Products.Add(new Models.Product { ProductId = 3, ProductName = "Durian", ProductQuantity = 20, ProductPrice = 10, ProductDescription = "Fortune favors the bold", ProductImage = "" });
                context.SaveChanges();
            }
            if (context.Users.Count() == 0)
            {
                context.Users.Add(new Models.User { userId = 1, email = "test@gmail.com", password = "pass", firstName = "John", lastName = "Doe" });
                context.Users.Add(new Models.User { userId = 2, email = "jane@gmail.com", password = "word", firstName = "Jane", lastName = "Doe" });
                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        public void EmptyProductTable()
        {
            foreach (var product in context.Products)
            {
                context.Products.Remove(product);
            }
            context.SaveChanges();
        }

        public void EmptyUserTable()
        {
            foreach (var user in context.Users)
            {
                context.Users.Remove(user);
            }
            context.SaveChanges();
        }
    }
}
