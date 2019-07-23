using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Models;
using System;
using System.Linq;

namespace ProductCatalog.Api.Data
{
    public class StoreDataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public StoreDataContext(DbContextOptions<StoreDataContext> options) : base(options) { }

        /// <summary>
        /// Metodo sobrecarregado para inclusao das propriedades CreateDate LastUpdateDate
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            DateTime saveTime = DateTime.Now;
            foreach (var entry in this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {

                if (entry.Metadata.FindProperty("LastUpdateDate") != null)
                {
                    if ((DateTime)entry.Property("LastUpdateDate")?.CurrentValue < saveTime)
                        entry.Property("LastUpdateDate").CurrentValue = saveTime;
                }

            }
            return base.SaveChanges();
        }

    }
}
