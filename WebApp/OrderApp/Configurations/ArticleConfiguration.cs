using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Models;

namespace OrderApp.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Description)
                .HasMaxLength(200);
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.HasIndex(x => x.Name)
                .IsUnique();
            builder.Property(x => x.Price)
                .IsRequired();
        }
    }
}
