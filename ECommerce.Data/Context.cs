using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
//using System.Data.Entity;

namespace ECommerce.Data
{
    public class Context : DbContext, IContext
    {

        public Context(DbContextOptions<Context> options) : base(options) {}

        public Context() {}

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public void DenoteProductModified(Product product)
        {
            Entry(product).State = EntityState.Modified;
        }

        public void DenoteUserModified(User user)
        {
            Entry(user).State = EntityState.Modified;
        }

        public Task CommitChangesAsync()
        {
            return SaveChangesAsync();
        }

        /**Product Table**/
        public async Task<Product> GetProductById(int id)
        {
            return await Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await Products.ToListAsync<Product>();
        }

        public async void UpdateProduct(Product prod)
        {
            Update(prod);
            SaveChanges();
        }

        /**User Table**/
        public async Task<User?> GetUserById(int id)
        {
            return await Users.FindAsync(id);
        }

        public async Task<bool> CreateNewUser(User usr)
        {
            bool emailTaken = Users.Any(u => u.email == usr.email);

            if (emailTaken)
            {
                return false;
            }
            else
            {
                Users.Add(usr);
                SaveChanges();
                return true;
            }
        }

        public async Task<User?> GetUserByEmailAndPassword(string email, string password)
        {
            email.Trim();
            password.Trim();
            return await Users.FirstOrDefaultAsync(u => u.email == email && u.password == password);
        }

        public async Task<User>UpdateUserPassword(string password, string email)
        {
            var user = await Users.Where(u => u.email == email).FirstOrDefaultAsync();

            // If no user was found, return null
            if (user == null) return null;

            // This statement returns null if the password submitted to the request is not a different password
            if (user.password == password) return null;

            user.password = password;
            DenoteUserModified(user);
            await CommitChangesAsync();
            return user;
        }
    }
}
