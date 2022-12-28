using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public  Task<Product> GetProductById(int id);
        public Task<IEnumerable<Product>> GetAllProducts();
    }
}
