using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaparaMessageBroker.Entity;

namespace PapararaMessageBroker
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
            base.OnModelCreating(modelBuilder);
        }
    }
}
