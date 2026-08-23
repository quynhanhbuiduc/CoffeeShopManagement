using CaféPourLaVie.Models;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.Data
{
    public class ApplicationDbContext : DbContext //Inherit from DbContext class to create a database context for the application

    {
        //Constructor that takes DbContextOptions and passes it to the base class constructor
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }


        public DbSet<Account> Accounts { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<ImportReceipt> ImportReceipts { get; set; }

        public DbSet<ImportDetail> ImportDetails { get; set; }

        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the one-to-one relationship between Employee and Account
            modelBuilder.Entity<Employee>()
                        .HasOne(e => e.Account)
                        .WithOne(a => a.Employee)
                        .HasForeignKey<Employee>(e => e.AccountId);


            modelBuilder.Entity<ImportReceipt>()
                        .HasOne(i => i.Account)
                        .WithMany()
                        .HasForeignKey(i => i.AccountId)
                        .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ImportDetail>()
                        .HasOne(d => d.ImportReceipt)
                        .WithMany(i => i.ImportDetails)
                        .HasForeignKey(d => d.ImportReceiptId)
                        .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ImportDetail>()
                        .HasOne(d => d.Product)
                        .WithMany()
                        .HasForeignKey(d => d.ProductId)
                        .OnDelete(DeleteBehavior.Restrict);


            // Configure the one-to-many relationship between Category and Product
            modelBuilder.Entity<PaymentMethod>()
                .HasData(
                    new PaymentMethod
                    {
                        PaymentMethodId = 1,
                        MethodName = "Tiền mặt"
                    },

                    new PaymentMethod
                    {
                        PaymentMethodId = 2,
                        MethodName = "Chuyển khoản"
                    },

                    new PaymentMethod
                    {
                        PaymentMethodId = 3,
                        MethodName = "Momo"
                    }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
