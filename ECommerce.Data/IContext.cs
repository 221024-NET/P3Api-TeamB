using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public interface IContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public void DenoteProductModified(Product product);
        public void DenoteUserModified(User user);

        public Task CommitChangesAsync();

        /**Product Table**/
        public Task<Product> GetProductById(int id);
        public Task<IEnumerable<Product>> GetAllProducts();

        public void UpdateProduct(Product prod);

        /**User Table**/
        public Task<User> CreateNewUser(User u);

        public Task<User> GetUserById(int id);

        public Task<User> GetUserLogin(string password, string email);
        public Task<User> UpdateUserPassword(string password, string email);
    }
}
