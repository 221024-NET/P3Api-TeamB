using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public class Context : DbContext, IContext
    {

        public Context(DbContextOptions<Context> options) : base (options)
        {  }
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
    }
}
