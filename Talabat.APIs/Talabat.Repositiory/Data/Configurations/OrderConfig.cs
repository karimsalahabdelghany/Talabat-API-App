using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Repositiory.Data.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status)
                .HasConversion(ostatus => ostatus.ToString(), Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus));

            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.OwnsOne(sh => sh.ShippingAddress, A => A.WithOwner());

            builder.HasOne(O => O.DeliveryMethod)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
