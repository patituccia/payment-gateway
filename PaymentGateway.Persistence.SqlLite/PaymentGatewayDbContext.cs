using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain;

namespace PaymentGateway.Persistence.SqlLite
{
    public class PaymentGatewayDbContext : DbContext
    {
        public PaymentGatewayDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Merchant> Merchants { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=paymentGateway.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .OwnsOne<Money>("Money");

            modelBuilder.Entity<Payment>()
                .HasOne<Merchant>()
                .WithMany()
                .HasForeignKey(p => p.MerchantId);

            modelBuilder.Entity<Merchant>().HasData(
                new Merchant(1, "Apple"),
                new Merchant(2, "Microsoft"),
                new Merchant(3, "Amazon"));
        }
    }
}
