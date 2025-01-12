﻿using ECommerce.Models;
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
        public Task<bool> CreateNewUser(User usr);

        public Task<User?> GetUserById(int id);

        public Task<User?> GetUserByEmailAndPassword(string email, string password);
    }
}
