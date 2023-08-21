using Microsoft.EntityFrameworkCore;
using OrderApp.Models;

namespace OrderApp.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
        public virtual DbSet<Article> Articles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderArticle> OrderArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderArticle>()
                .HasKey(op => new { op.OrderId, op.ArticleId });

            //base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
        }
    }
}
