using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
//using System.Data.Entity;

namespace ECommerce.Data
{
    public class Context : DbContext, IContext
    {

        public Context(DbContextOptions<Context> options) : base(options)
        {



        }


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

        public async Task<User> GetUserById(int id)
        {
            return await Users.FindAsync(id);
        }

        public async Task<User> CreateNewUser(User u)
        {
            Add(u);
            SaveChanges();


            return u;
        }

        public async Task<User>GetUserLogin(string password, string email)
        {
            

            var user = Users.Where(usr=>usr.email ==email && usr.password ==password).FirstOrDefault();

            if (user == null) 
            {
                return null;
            }
            return user;

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
