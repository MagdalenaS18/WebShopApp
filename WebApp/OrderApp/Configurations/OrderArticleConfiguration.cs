using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Models;

namespace OrderApp.Configurations
{
    public class OrderArticleConfiguration : IEntityTypeConfiguration<OrderArticle>
    {
        public void Configure(EntityTypeBuilder<OrderArticle> builder)
        {
            builder.HasOne(o => o.Order)
                .WithMany(oa => oa.OrderArticles)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Article)
                .WithMany(oa => oa.OrderArticles)
                .HasForeignKey(a => a.ArticleId);
        }
    }
}
