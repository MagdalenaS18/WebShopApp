using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Models;

namespace OrderApp.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Address)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.Comment)
                .HasMaxLength(200);
        }
    }
}
