using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public class BookShopDBContext:IdentityDbContext
    {
        public BookShopDBContext() { }
        public BookShopDBContext(DbContextOptions<BookShopDBContext>db) : base(db) 
        { 
        
        } 
    
        public virtual DbSet<Book> Books { get; set; }
        public virtual  DbSet<Catalog> Catalogs { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<OrderDetails> order_Details { get; set; } 
        public virtual DbSet<Order> orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<OrderDetails>().HasKey("book_id", "order_id");

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name="admin", NormalizedName="ADMIN" }, 
                new IdentityRole() { Name="customer", NormalizedName="CUSTOMER" }

                );
        }




    }
}
