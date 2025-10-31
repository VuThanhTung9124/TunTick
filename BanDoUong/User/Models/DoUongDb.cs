namespace BanDoUong.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DoUongDb : DbContext
    {
        public DoUongDb()
            : base("name=DoUongDb")
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.price)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Product>()
                .Property(e => e.old_price)
                .HasPrecision(12, 2);
        }
    }
}
